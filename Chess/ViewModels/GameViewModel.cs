using System.Collections.Generic;
using System.Threading.Tasks;
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
                Icons = value.GetIcons();
            }
        }

        #endregion

        #region Свойство Icons

        private IHaveIcon?[,] _icons = new IHaveIcon[8,8];

        public IHaveIcon?[,] Icons
        {
            get => _icons;
            set => Set(ref _icons, value);
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
        public void Move()
        {
            if (StartPoint is { } startPoint && EndPoint is { } endPoint)
            {

                MoveInfo = ChessBoard.Move(startPoint, endPoint);
                if (MoveInfo.ChangePositions != null)
                {
                    ChessBoard.WhoseMove = ChessBoard.WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;

                    if (_currentBoardId + 1 != _listChessBoards.Count)
                    {
                        _listChessBoards = _listChessBoards.GetRange(0, _currentBoardId+1);
                        _listChessBoards.Add((ChessBoard)ChessBoard.Clone());
                        _currentBoardId += 1;
                    }
                    else
                    {
                        _listChessBoards.Add((ChessBoard)ChessBoard.Clone());
                        _currentBoardId += 1;
                    }
                }

            }
        }
        private async void SetNewHintsChessAsync(Point startPoint)
        {

            await Task.Run(() =>
            {
                Hints = ChessBoard.GetHintsForPiece(startPoint);
            });
        }

    }
}
