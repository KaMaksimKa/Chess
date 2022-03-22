using System;
using System.Threading.Tasks;
using System.Windows;
using Chess.Models;
using Chess.ViewModels.Base;
using Point = System.Drawing.Point;

namespace Chess.ViewModels
{
    internal class GameViewModel:ViewModel
    {
  
        public ChessBoard ChessBoard { get; set ; }= new ChessBoard();

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
                ChessBoard.Move(pos, newPos);

                ChessBoard.WhoseMove = ChessBoard.WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;

                ChangePos = new ChangePosition{StartPoint = new Point(pos.X, pos.Y),
                                                 EndPoint = new Point(newPos.X, newPos.Y)};
            }
            catch (ApplicationException)
            {
                ChangePos = new ChangePosition { StartPoint = new Point(pos.X, pos.Y),
                                                    EndPoint = new Point(pos.X, pos.Y) };
            }

            StartPoint = null;
            EndPoint = null;
        }
        private async void SetNewHintsChessAsync(Point startPoint)
        {

            await Task.Run(() =>
            {
                bool[,] hintsForMove = new bool[8, 8];
                bool[,] hintsForKill = new bool[8, 8];

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        hintsForMove[i, j] = ChessBoard.IsMove(startPoint, new Point(i, j));
                        hintsForKill[i, j] = ChessBoard.IsKill(startPoint, new Point(i, j));
                    }
                }
                Hints = new HintsChess { IsHintsForKill = hintsForKill, IsHintsForMove = hintsForMove };
            });
            

           
        }

    }
}
