using System;
using System.Collections;
using System.Collections.Generic;
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

        #region Свойство ChangePos
        private ChangePosition? _changePos;
        public ChangePosition? ChangePos
        {
            get => _changePos;
            set => Set(ref _changePos, value);
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
            if (StartPoint==null || EndPoint==null)
                return;
            
            var pos = (Point)StartPoint;
            var newPos = (Point)EndPoint;
            try
            {
                IEnumerable<(Point,Point)> moves =  ChessBoard.Move((byte)pos.X, (byte)pos.Y, (byte)newPos.X, (byte)newPos.Y);

                ChessBoard.WhoseMove = ChessBoard.WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;

                StartPoint = null;
                foreach (var (startPoint,endPoint) in moves)
                {
                    ChangePos = new ChangePosition(startPoint,endPoint);
                }
            }

            catch (ApplicationException)
            {
                ChangePos = new ChangePosition { StartPoint = new Point(pos.X, pos.Y),
                                                    EndPoint = new Point(pos.X, pos.Y) };
            }

            EndPoint = null;


        }
        private async void SetNewHintsChessAsync(Point startPoint)
        {

            await Task.Run(() =>
            {
                bool[,] hintsForMove = new bool[8, 8];
                bool[,] hintsForKill = new bool[8, 8];

                for (byte i = 0; i < 8; i++)
                {
                    for (byte j = 0; j < 8; j++)
                    {
                        hintsForMove[i, j] = ChessBoard.IsMove((byte)startPoint.X, (byte)startPoint.Y, i, j);
                        hintsForKill[i, j] = ChessBoard.IsKill((byte)startPoint.X, (byte)startPoint.Y, i, j);
                    }
                }
                Hints = new HintsChess { IsHintsForKill = hintsForKill, IsHintsForMove = hintsForMove };
            });
            

           
        }
        

    }
}
