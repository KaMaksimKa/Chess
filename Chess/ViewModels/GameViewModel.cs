using System.Threading.Tasks;
using Chess.Models;
using Chess.Models.PiecesChess.Base;
using Chess.ViewModels.Base;
using Point = System.Drawing.Point;

namespace Chess.ViewModels
{
    internal class GameViewModel:ViewModel
    {


        private ChessBoard _chessBoard;
        public ChessBoard ChessBoard
        {
            get => _chessBoard;
            set
            {
                _chessBoard = value;
                Icons = value.GetIcons();
            }
        } 

        #region Свойство Icons

        private IHaveIcon?[,] _icons;

        public IHaveIcon?[,] Icons
        {
            get => _icons;
            set => Set(ref _icons, value);
        }

        public GameViewModel()
        {
            ChessBoard = new ChessBoard();
        }
        #endregion

        #region Свойство MoveInfo
        private MoveInfo _moveInfo;
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
        public void Move()
        {
            if (StartPoint is { } startPoint && EndPoint is { } endPoint)
            {

                MoveInfo = ChessBoard.Move(startPoint, endPoint);
                if (MoveInfo.ChangePositions != null)
                {
                    ChessBoard.WhoseMove = ChessBoard.WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;
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
