using System;
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
    internal abstract class GameViewModel:ViewModel
    {
        private bool _isGameGoing;
        public bool IsGameGoing
        {
            get => _isGameGoing;
            private set
            {
                Set(ref _isGameGoing, value);
                IsChangePlayers = !value;
            }
        }

        private List<GameBoard> _listChessBoards = new List<GameBoard>();
        private int _currentBoardId;

        protected  IPlayer FirstPlayer = new SelfPlayer(TeamEnum.WhiteTeam);
        protected  IPlayer SecondPlayer = new BotPlayer(TeamEnum.BlackTeam,2);
        protected TeamEnum FirstPlayerTeam = TeamEnum.WhiteTeam;
        protected TeamEnum SecondPlayerTeam = TeamEnum.BlackTeam;


        private GameBoard _gameBoard = new ChessBoard(TeamEnum.WhiteTeam);
        protected GameBoard GameBoard
        {
            get => _gameBoard;
            set
            {
                _gameBoard = value;
                BoardForDraw = new BoardForDraw { Icons = value.GetIcons(),LastMoveInfo = value.LastMoveInfo};
                if (!IsGameGoing)
                {
                    _listChessBoards.Add((GameBoard)GameBoard.Clone());
                    _currentBoardId = 0;
                }
            }
        }

        public List<TypePlayer> AvailablePlayers { get;protected init; } = new List<TypePlayer>
        {
            TypePlayer.SelfPlayer,
            TypePlayer.Bot1,
            TypePlayer.Bot2
        };
        private bool _isChangePlayers = true;
        public bool IsChangePlayers
        {
            get => _isChangePlayers;
            set => Set(ref _isChangePlayers, value);
        }

        private TypePlayer _selectedFirstPlayer = TypePlayer.SelfPlayer;
        public TypePlayer SelectedFirstPlayer
        {
            get => _selectedFirstPlayer;
            set => Set(ref _selectedFirstPlayer, value);
        }

        private TypePlayer _selectedSecondPlayer = TypePlayer.Bot2;
        public TypePlayer SelectedSecondPlayer
        {
            get => _selectedSecondPlayer;
            set => Set(ref _selectedSecondPlayer, value);
        }


        #region Свойство StartPoint
        private Point? _startPoint;
        public Point? StartPoint
        {
            get => _startPoint;
            set
            {
                Set(ref _startPoint, value);
                if (IsGameGoing && GetCurrentPlayer()  is SelfPlayer selfPlayer)
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
                if (IsGameGoing && GetCurrentPlayer() is SelfPlayer selfPlayer )
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
                if (IsGameGoing && GetCurrentPlayer() is SelfPlayer selfPlayer)
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
                GameBoard = (GameBoard)_listChessBoards[_currentBoardId].Clone();
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

        private bool CanStartGameCommandExecute(object p) => !IsGameGoing;

        private void OnStartGameCommandExecuted(object p)
        {
            SetStartGameState();
            IsGameGoing = true;
            Move();
            
        }

        #endregion

        #region Команда GiveUpPlayerCommand 

        public ICommand GiveUpPlayerCommand { get; }

        private bool CanGiveUpPlayerCommandExecute(object p) => IsGameGoing&&GetCurrentPlayer() is SelfPlayer;

        private void OnGiveUpPlayerCommandExecuted(object p)
        {
           EndGame(GameBoard.WhoseMove);
        }


        #endregion

        #region Команда ChangePlayerTeamsCommand 

        public ICommand ChangePlayerTeamsCommand { get; }

        private bool CanChangePlayerTeamsCommandExecute(object p) => !IsGameGoing;

        private void OnChangePlayerTeamsCommandExecuted(object p)
        {
            (FirstPlayerTeam, SecondPlayerTeam) = (SecondPlayerTeam, FirstPlayerTeam);
            SetStartGameState();
        }


        #endregion

        #endregion

        protected GameViewModel()
        {
            #region Команды
            NextStateStateChessBoardCommand = new LambdaCommand(OnNextStateStateChessBoardCommandExecuted,
                                                                CanNextStateStateChessBoardCommandExecute);

            PrevStateStateChessBoardCommand = new LambdaCommand(OnPrevStateStateChessBoardCommandExecuted,
                                                                CanPrevStateStateChessBoardCommandExecute);

            StartGameCommand = new LambdaCommand(OnStartGameCommandExecuted,
                                                 CanStartGameCommandExecute);
            ChangePlayerTeamsCommand = new LambdaCommand(OnChangePlayerTeamsCommandExecuted, 
                                                         CanChangePlayerTeamsCommandExecute);
            GiveUpPlayerCommand = new LambdaCommand(OnGiveUpPlayerCommandExecuted, CanGiveUpPlayerCommandExecute);

            #endregion

            SetStartGameState();
        }
        public IPlayer GetCurrentPlayer()
        {
            return FirstPlayer.Team == GameBoard.WhoseMove ? FirstPlayer : SecondPlayer;
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
        private void MovedPlayer(Point startPoint, Point endPoint)
        {
            if (IsGameGoing)
            {
                GameBoard.MakeMove(startPoint, endPoint);
            }
        }
        private void MovedBoard(MoveInfo moveInfo)
        {
            var queue = new Queue<MoveInfo>();
            queue.Enqueue(moveInfo);
            if (moveInfo.IsMoved)
            {

                if (GetCurrentPlayer() is SelfPlayer)
                {
                    SaveStateChessBoard();
                }
                MoveInfoQueue = queue;
                Move();
            }
            else
            {
                MoveInfoQueue = queue;
            }
        }
        private void EndGame(TeamEnum? teamEnum)
        {
            IsGameGoing = false;
            if (teamEnum is { } team)
            {
                MessageBox.Show($"Проиграл {team}");
            }
            else
            {
                MessageBox.Show("Ничья ");
            }
        }

        private void SetStartGameState()
        {
            GameBoard board = GetNewBoard();
            board.ChessBoardMovedEvent += MovedBoard;
            board.EndGameEvent += EndGame;
            GameBoard = board;

            SetFirstPlayer(SelectedFirstPlayer);
            SetSecondPlayer(SelectedSecondPlayer);
        }

        protected abstract GameBoard GetNewBoard();

        protected SelfPlayer GetNewSelfPlayer(TeamEnum team)
        {
            var selfPlayer = new SelfPlayer(team);
            selfPlayer.MovedEvent += MovedPlayer;
            selfPlayer.SetHintsForMoveEvent += (hintsChess =>Hints= hintsChess);
            selfPlayer.GetSelectedPieceEvent += selectedPiece => SelectedPiece = selectedPiece;
            if (GameBoard is ChessBoard)
            {
                selfPlayer.SetSelectedPieceEvent += piece => ((ChessBoard) GameBoard).SetReplasementPiece(piece);
            }

            return selfPlayer;
        }
        protected BotChessPlayer GetNewBotChessPlayer(TeamEnum team,int depth)
        {
            var botChessPlayer = new BotChessPlayer(team, depth);
            botChessPlayer.MovedEvent += MovedPlayer;
            if (GameBoard is ChessBoard)
            {
                botChessPlayer.SetSelectedPieceEvent += piece => ((ChessBoard)GameBoard).SetReplasementPiece(piece);
            }
           
            return botChessPlayer;
        }
        protected BotPlayer GetNewBotPlayer(TeamEnum team, int depth)
        {
            var botPlayer = new BotPlayer(team, depth);
            botPlayer.MovedEvent += MovedPlayer;
            if (GameBoard is ChessBoard)
            {
                botPlayer.SetSelectedPieceEvent += piece => ((ChessBoard) GameBoard).SetReplasementPiece(piece);
            }
            return botPlayer;
        }
        protected void SetFirstPlayer(TypePlayer typePlayer)
        {
            FirstPlayer = GetNewPlayer(typePlayer, FirstPlayerTeam);
        }
        protected void SetSecondPlayer(TypePlayer typePlayer)
        {
            SecondPlayer = GetNewPlayer(typePlayer, SecondPlayerTeam);
        }
        private IPlayer GetNewPlayer(TypePlayer typePlayer,TeamEnum team)
        {
            return typePlayer switch
            {
                TypePlayer.SelfPlayer => GetNewSelfPlayer(team),
                TypePlayer.Bot1 => GetNewBotPlayer(team,1),
                TypePlayer.Bot2 => GetNewBotPlayer(team, 2),
                TypePlayer.Bot3 => GetNewBotPlayer(team, 3),
                TypePlayer.Bot4 => GetNewBotPlayer(team, 4),
                TypePlayer.Bot5 => GetNewBotPlayer(team, 5),
                TypePlayer.Bot6 => GetNewBotPlayer(team, 6),
                TypePlayer.ChessBot1 => GetNewBotChessPlayer(team, 1),
                TypePlayer.ChessBot2 => GetNewBotChessPlayer(team, 2),
                TypePlayer.ChessBot3 => GetNewBotChessPlayer(team, 3),
                TypePlayer.ChessBot4 => GetNewBotChessPlayer(team, 4),
                _ => throw new ArgumentOutOfRangeException(nameof(typePlayer), typePlayer, null)
            };
        }
    }
}
