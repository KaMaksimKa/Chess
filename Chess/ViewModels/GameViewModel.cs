using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Chess.Infrastructure.Commands;
using Chess.Models;
using Chess.Models.PiecesChess.Base;
using Chess.ViewModels.Base;
using Point = System.Drawing.Point;

namespace Chess.ViewModels
{
    internal class GameViewModel:ViewModel
    {
        private List<ChessBoard> _listChessBoards = new List<ChessBoard>();
        private int _currentBoardId;

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

        #region Свойство MoveInfo
        private MoveInfo _moveInfo = new MoveInfo();
        public MoveInfo MoveInfo
        {
            get => _moveInfo;
            set => Set(ref _moveInfo, value);
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
                if (value is not null)
                {
                    SetNewHintsChessAsync((Point)value);
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
                Move();
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
            }
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
            #endregion

            ChessBoard = new ChessBoard();

            _listChessBoards.Add((ChessBoard)ChessBoard.Clone());
            _currentBoardId = 0;

        }

        public bool IsMate()
        {
            for (byte i = 0; i < 8; i++)
            {
                for (byte j = 0; j < 8; j++)
                {
                    if (GetHintsChess(new Point(i, j)).IsHintsForMove.Count > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public HintsChess GetHintsChess(Point point)
        {
            List<Point> hintsForMove = new List<Point>();
            List<Point> hintsForKill = new List<Point>();

            for (byte i = 0; i < 8; i++)
            {
                for (byte j = 0; j < 8; j++)
                {
                    if (ChessBoard.IsMove(point, new Point(i, j)) is { } moveInfo)
                    {
                        if (moveInfo.KillPoint != null)
                        {
                            hintsForKill.Add(new Point(i, j));
                        }
                        else if (moveInfo.ChangePositions != null)
                        {
                            hintsForMove.Add(new Point(i, j));
                        }
                    }
                }
            }
            return new HintsChess { IsHintsForKill = hintsForKill, IsHintsForMove = hintsForMove };
        }
        public void Move()
        {
            if (StartPoint is { } startPoint && EndPoint is { } endPoint)
            {
                if (ChessBoard.Move(startPoint, endPoint) is { } moveInfo)
                {
                    MoveInfo = moveInfo;
                    
                    ChessBoard.WhoseMove = ChessBoard.WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;


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

                }
                else
                {
                    MoveInfo = new MoveInfo();
                }

            }
        }
        private async void SetNewHintsChessAsync(Point startPoint)
        {

            await Task.Run(() =>
            {
                Hints = GetHintsChess(startPoint);
            });
        }

    }
}
