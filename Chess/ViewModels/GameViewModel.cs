using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Chess.Infrastructure.Commands;
using Chess.Models;
using Chess.Models.Boards;
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

        private List<GameBoard> _listChessBoards = new List<GameBoard>();
        private int _currentBoardId;

        private readonly IPlayer _firstPlayer;
        private readonly IPlayer _secondPlayer;

        private GameBoard _gameBoard = new ChessBoard(TeamEnum.WhiteTeam);
        private GameBoard GameBoard
        {
            get => _gameBoard;
            set
            {
                _gameBoard = value;
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
                GameBoard = (ChessBoard)_listChessBoards[_currentBoardId].Clone();
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
                GameBoard = (GameBoard)_listChessBoards[_currentBoardId].Clone();
                Move();
            }
        }

        #endregion

        #region Команда StartNewGameCommand 

        public ICommand StartGameCommand { get; }

        private bool CanStartGameCommandExecute(object p) => true;

        private void OnStartGameCommandExecuted(object p)
        {
            GameBoard = (GameBoard)_listChessBoards[0].Clone();

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

            
            GameBoard = GetNewCheckersBoard(TeamEnum.WhiteTeam);
            _firstPlayer = GetNewSelfPlayer();
            _secondPlayer = GetNewSelfPlayer();
            
            _listChessBoards.Add((GameBoard)GameBoard.Clone());
            _currentBoardId = 0;
            

        }
        public IPlayer GetCurrentPlayer()
        {
            return _firstPlayer.Team == GameBoard.WhoseMove ? _firstPlayer : _secondPlayer;
        }
        public void SaveStateChessBoard()
        {
            if (_currentBoardId + 1 != _listChessBoards.Count)
            {
                _listChessBoards = _listChessBoards.GetRange(0, _currentBoardId + 1);
                _listChessBoards.Add((GameBoard)GameBoard.Clone());
                _currentBoardId += 1;
            }
            else
            {
                _listChessBoards.Add((GameBoard)GameBoard.Clone());
                _currentBoardId += 1;
            }
        }
        private void Move()
        {
            Task.Run(() => GetCurrentPlayer().CanMovePlayer(GameBoard));
        }
        public void MovedPlayer(Point startPoint, Point endPoint)
        {
            if (_isGameGoing)
            {
                GameBoard.MakeMove(startPoint, endPoint);
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
        private GameBoard GetNewChessBoard(TeamEnum team)
        {
            var chessBoard = new ChessBoard(team);
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
        private GameBoard GetNewCheckersBoard(TeamEnum team)
        {
            var checkersBoard = new CheckersBoards(team);
            checkersBoard.ChessBoardMovedEvent += MovedBoard;
            checkersBoard.EndGameEvent += EndGame;

            return checkersBoard;
        }
        private SelfPlayer GetNewSelfPlayer()
        {
            var selfPlayer = new SelfPlayer(TeamEnum.WhiteTeam);
            selfPlayer.MovedEvent += MovedPlayer;
            selfPlayer.SetHintsForMoveEvent += (hintsChess =>Hints= hintsChess);
            selfPlayer.GetSelectedPieceEvent += selectedPiece => SelectedPiece = selectedPiece;
            if (GameBoard is ChessBoard chessBoard)
            {
                selfPlayer.SetSelectedPieceEvent += piece => chessBoard.SetReplasementPiece(piece);
            }
            return selfPlayer;
        }
        private BotPlayer GetNewBotPlayer()
        {
            var botPlayer = new BotPlayer(TeamEnum.BlackTeam);
            botPlayer.MovedEvent += MovedPlayer;
            if (GameBoard is ChessBoard chessBoard)
            {
                botPlayer.SetSelectedPieceEvent += piece => chessBoard.SetReplasementPiece(piece);
            }
            return botPlayer;
        }
        
    }
}
