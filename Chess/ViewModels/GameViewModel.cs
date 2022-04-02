using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Chess.Infrastructure.Commands;
using Chess.Models;
using Chess.Models.Boards.Base;
using Chess.Models.Players;
using Chess.Models.Players.Base;
using Chess.ViewModels.Base;
using Point = System.Drawing.Point;

namespace Chess.ViewModels
{
    internal class GameViewModel:ViewModel
    {
        private List<ChessBoard> _listChessBoards = new List<ChessBoard>();
        private int _currentBoardId;

        private readonly Player _firstPlayer;
        private readonly Player _secondPlayer;

        #region Свойство ChessBoard

        private ChessBoard _chessBoard = new ChessBoard();
        public ChessBoard ChessBoard
        {
            get => _chessBoard;
            set
            {
                _chessBoard = value;
                BoardForDraw = new BoardForDraw { Icons = value.GetIcons(),LastMoveInfo = value.LastMoveInfo};
            }
        }

        #endregion

        #region Свойство BoardForDraw

        private BoardForDraw _boardForDraw = new BoardForDraw();

        public BoardForDraw BoardForDraw
        {
            get => _boardForDraw;
            set => Set(ref _boardForDraw, value);
        }

        #endregion

        #region Свойство MoveInfoQueue
        private Queue<MoveInfo> _moveInfoQueue = new Queue<MoveInfo>();
        public Queue<MoveInfo> MoveInfoQueue
        {
            get => _moveInfoQueue;
            set => Set(ref _moveInfoQueue, value);
        }
        #endregion

        #region Свойство StartPoint
        private Point? _startPoint;
        public Point? StartPoint
        {
            get => _startPoint;
            set
            {
                Set(ref _startPoint, value);

                var currentPlayer = _firstPlayer.Team == ChessBoard.WhoseMove ? _firstPlayer : _secondPlayer;
                if (currentPlayer is SelfPlayer selfPlayer)
                {
                    selfPlayer.StartPoint = value;
                    SetNewHintsChessAsync(value);
                }
            }
        }

        #endregion

        #region Свойство EndPoint
        private Point? _endPoint;
        public Point? EndPoint
        {
            get => _endPoint;
            set
            {
                Set(ref _endPoint, value);
                var currentPlayer = _firstPlayer.Team == ChessBoard.WhoseMove ? _firstPlayer : _secondPlayer;
                if (currentPlayer is SelfPlayer selfPlayer)
                {
                    selfPlayer.EndPoint = value;
                    MoveAsync();
                }
                if (StartPoint == null)
                {
                    Hints = new HintsChess();
                }
                
            }
        }
        #endregion

        #region Свойство Hints

        private HintsChess _hints = new HintsChess();

        public HintsChess Hints
        {
            get => _hints;
            set => Set(ref _hints, value);
        }

        #endregion

        #region Команды

        #region Команда NextStateStateChessBoard 
        public ICommand NextStateStateChessBoardCommand { get; }

        private bool CanNextStateStateChessBoardCommandExecute(object p) => true;

        private void OnNextStateStateChessBoardCommandExecuted(object p)
        {
            if (_currentBoardId + 1 < _listChessBoards.Count)
            {
                _currentBoardId += 1;
                ChessBoard = (ChessBoard)_listChessBoards[_currentBoardId].Clone();
                _firstPlayer.ChessBoard = ChessBoard;
                _secondPlayer.ChessBoard = ChessBoard;
                MoveAsync();
            }
        }

        #endregion

        #region Команда PrevStateStateChessBoard 

        public ICommand PrevStateStateChessBoardCommand { get; }

        private bool CanPrevStateStateChessBoardCommandExecute(object p) => true;

        private void OnPrevStateStateChessBoardCommandExecuted(object p)
        {
            if (_currentBoardId - 1 >= 0)
            {
                _currentBoardId -= 1;
                ChessBoard = (ChessBoard)_listChessBoards[_currentBoardId].Clone();
                _firstPlayer.ChessBoard = ChessBoard;
                _secondPlayer.ChessBoard = ChessBoard;
                MoveAsync();
            }
        }

        #endregion

        #region Команда StartGameCommand 

        public ICommand StartGameCommand { get; }

        private bool CanStartGameCommandExecute(object p) => true;

        private void OnStartGameCommandExecuted(object p)
        {
            if (_listChessBoards.Count > 0)
            {
                ChessBoard = (ChessBoard)_listChessBoards[0].Clone();
                _firstPlayer.ChessBoard = ChessBoard;
                _secondPlayer.ChessBoard = ChessBoard;
            }
            MoveAsync();
            
        }
        

        #endregion

        #endregion

        public GameViewModel()
        {
            #region Команды
            NextStateStateChessBoardCommand = new LambdaCommand(OnNextStateStateChessBoardCommandExecuted,
                                                                CanNextStateStateChessBoardCommandExecute);

            PrevStateStateChessBoardCommand = new LambdaCommand(OnPrevStateStateChessBoardCommandExecuted,
                                                                CanPrevStateStateChessBoardCommandExecute);

            StartGameCommand = new LambdaCommand(OnStartGameCommandExecuted,
                CanStartGameCommandExecute);
            #endregion
            
            ChessBoard = new ChessBoard();
            ChessBoard.ChessBoardMovedEvent += Moved;


            _firstPlayer = new SelfPlayer(TeamEnum.WhiteTeam, ChessBoard);
            _secondPlayer = new BotPlayer(TeamEnum.BlackTeam, ChessBoard);

            _listChessBoards.Add((ChessBoard)ChessBoard.Clone());
            _currentBoardId = 0;

        }
        private async void MoveAsync()
        {
            await Task.Run(() =>
            {
                var currentPlayer = _firstPlayer.Team == ChessBoard.WhoseMove ? _firstPlayer : _secondPlayer;

                var changePos = currentPlayer.Move();
                if (changePos is { } pos)
                {
                    var (startPoint, endPoint) = pos;
                    ChessBoard.Move(startPoint, endPoint);
                }
            });
        }
        public void Moved(MoveInfo? moveInfo)
        {

            if (moveInfo is { })
            {
                var currentPlayer = _firstPlayer.Team == ChessBoard.WhoseMove ? _firstPlayer : _secondPlayer;

                if (currentPlayer is SelfPlayer selfPlayer)
                {
                    selfPlayer.StartPoint = null;
                    selfPlayer.EndPoint = null;
                    
                }
                SaveStateChessBoard();

                var queue = new Queue<MoveInfo>();
                queue.Enqueue(moveInfo);
                MoveInfoQueue = queue;

                if (IsMate())
                {
                    if (ChessBoard.IsCheck(null))
                    {
                        MessageBox.Show("Шах и мат ");
                    }
                    else
                    {
                        MessageBox.Show("Ничья ");
                    }
                }
                else
                {
                    MoveAsync();
                }
            }
            else
            {
                var queue = new Queue<MoveInfo>();
                queue.Enqueue(new MoveInfo());
                MoveInfoQueue = queue;
            }
        }
        public bool IsMate()
        {
            for (byte i = 0; i < 8; i++)
            {
                for (byte j = 0; j < 8; j++)
                {
                    var hints = GetHintsChess(new Point(i, j));
                    if (hints.IsHintsForMove.Count > 0 || hints.IsHintsForKill.Count > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public void SaveStateChessBoard()
        {
            if (_currentBoardId + 1 != _listChessBoards.Count)
            {
                _listChessBoards = _listChessBoards.GetRange(0, _currentBoardId + 1);
                _listChessBoards.Add((ChessBoard)ChessBoard.Clone());
                _currentBoardId += 1;
            }
            else
            {
                _listChessBoards.Add((ChessBoard)ChessBoard.Clone());
                _currentBoardId += 1;
            }
        }
        public HintsChess GetHintsChess(Point? startPoint)
        {
            List<Point> hintsForMove = new List<Point>();
            List<Point> hintsForKill = new List<Point>();

            var moves = ChessBoard.GetMovesForPiece(startPoint);
            if (moves != null)
            {
                foreach (var ((_, _), moveInfo) in moves)
                {
                    if (moveInfo.KillPoint != null)
                    {
                        hintsForKill.Add(moveInfo.Move.EndPoint);
                    }
                    else if (moveInfo.ChangePositions != null)
                    {
                        hintsForMove.Add(moveInfo.Move.EndPoint);
                    }
                }
            }

            return new HintsChess { IsHintsForKill = hintsForKill, IsHintsForMove = hintsForMove };
        }
        private void SetNewHintsChessAsync(Point? startPoint)
        {

            
                Hints = GetHintsChess(startPoint);
            
        }

    }
}
