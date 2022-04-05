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
        private bool _isGameGoing;

        private List<ChessBoard> _listChessBoards = new List<ChessBoard>();
        private int _currentBoardId;

        private readonly IPlayer _firstPlayer;
        private readonly IPlayer _secondPlayer;

        private ChessBoard _chessBoard = new ChessBoard();
        private ChessBoard ChessBoard
        {
            get => _chessBoard;
            set
            {
                _chessBoard = value;
                BoardForDraw = new BoardForDraw { Icons = value.GetIcons(),LastMoveInfo = value.LastMoveInfo};
            }
        }

        #region Свойство StartPoint
        private Point? _startPoint;
        public Point? StartPoint
        {
            get => _startPoint;
            set
            {
                Set(ref _startPoint, value);
                if (_isGameGoing && GetCurrentPlayer() is SelfPlayer selfPlayer)
                {
                    selfPlayer.StartPoint = value;
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
                if (_isGameGoing && GetCurrentPlayer() is SelfPlayer selfPlayer )
                {
                    selfPlayer.EndPoint = value;
                }
                else
                {
                    if (StartPoint is { } startPoint && EndPoint is { } endPoint)
                    {
                        Task.Run(() => SetEmptyMoveInfoQueue(startPoint,endPoint));
                    }
                   
                }
            }
        }
        #endregion

        # region Свойство SelectedPiece

        private ChoicePiece _selectedPiece = new ChoicePiece();

        public ChoicePiece SelectedPiece
        {
            get => _selectedPiece;
            set
            {
                Set(ref _selectedPiece, value);
                if (_isGameGoing && GetCurrentPlayer() is SelfPlayer selfPlayer)
                {
                    Task.Run(() => selfPlayer.SetSelectPiece(value));
                }
            }
        }

        #endregion

        #region Свойство MoveInfoQueue
        private Queue<MoveInfo> _moveInfoQueue = new Queue<MoveInfo>();
        public Queue<MoveInfo> MoveInfoQueue
        {
            get => _moveInfoQueue;
            set => Set(ref _moveInfoQueue, value);
        }

        private void SetEmptyMoveInfoQueue(Point startPoint, Point endPoint)
        {
            var queue = new Queue<MoveInfo>();
            queue.Enqueue(new MoveInfo
            {
                IsMoved = false,
                Move = new ChangePosition(startPoint, endPoint)
            });
            MoveInfoQueue = queue;
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
                Move();
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
                Move();
            }
        }

        #endregion

        #region Команда StartNewGameCommand 

        public ICommand StartGameCommand { get; }

        private bool CanStartGameCommandExecute(object p) => true;

        private void OnStartGameCommandExecuted(object p)
        {
            ChessBoard = (ChessBoard)_listChessBoards[0].Clone();

            _listChessBoards = _listChessBoards.GetRange(0, 1);
            _currentBoardId = 0;

            _isGameGoing = true;
            Move();
            
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

            ChessBoard = GetNewChessBoard();
            _firstPlayer = GetNewSelfPlayer();
            _secondPlayer = GetNewSelfPlayer();
            
            _listChessBoards.Add((ChessBoard)ChessBoard.Clone());
            _currentBoardId = 0;

        }
        public IPlayer GetCurrentPlayer()
        {
            return _firstPlayer.Team == ChessBoard.WhoseMove ? _firstPlayer : _secondPlayer;
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
        private void Move()
        {
            Task.Run(() => GetCurrentPlayer().CanMovePlayer(ChessBoard));
        }
        public void MovedPlayer(Point startPoint, Point endPoint)
        {
            if (_isGameGoing)
            {
                ChessBoard.Move(startPoint, endPoint);
            }
        }
        public void MovedBoard(MoveInfo moveInfo)
        {
            var queue = new Queue<MoveInfo>();
            queue.Enqueue(moveInfo);
            if (moveInfo.IsMoved)
            {
                SaveStateChessBoard();
                MoveInfoQueue = queue;
                Move();
            }
            else
            {
                MoveInfoQueue = queue;
            }
        }
        public bool IsMate()
        {
            for (byte i = 0; i < 8; i++)
            {
                for (byte j = 0; j < 8; j++)
                {
                    var movesPiece = ChessBoard.GetMovesForPiece(new Point(i,j),ChessBoard);
                    if (movesPiece.Count>0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void EndGame(TeamEnum? teamEnum)
        {
            _isGameGoing = false;
            if (teamEnum is { } team)
            {
                MessageBox.Show($"Шах и мат, проиграл {team}");
            }
            else
            {
                MessageBox.Show("Ничья ");
            }
        }
        private ChessBoard GetNewChessBoard()
        {
            var chessBoard = new ChessBoard();
            chessBoard.ChessBoardMovedEvent += MovedBoard;
            chessBoard.EndGameEvent += EndGame;
           

            chessBoard.ChoiceReplacementPieceEvent += (pieces, whereReplace) =>
            {
                GetCurrentPlayer().SelectPiece(new ChoicePiece
                {
                    PiecesList = pieces, WhereReplace = whereReplace
                });
            };
            
            return chessBoard;
        }
        private SelfPlayer GetNewSelfPlayer()
        {
            var selfPlayer = new SelfPlayer(TeamEnum.WhiteTeam);
            selfPlayer.MovedEvent += MovedPlayer;
            selfPlayer.SetHintsForMoveEvent += (hintsChess =>Hints= hintsChess);
            selfPlayer.SetSelectedPieceEvent += piece => ChessBoard.SetReplasementPiece(piece);
            selfPlayer.GetSelectedPieceEvent += selectedPiece => SelectedPiece = selectedPiece;
            return selfPlayer;
        }
        private BotPlayer GetNewBotPlayer()
        {
            var botPlayer = new BotPlayer(TeamEnum.BlackTeam);
            botPlayer.MovedEvent += MovedPlayer;
            botPlayer.SetSelectedPieceEvent += piece => ChessBoard.SetReplasementPiece(piece);
            return botPlayer;
        }
        
    }
}
