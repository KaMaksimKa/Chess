using System;
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
            set => Set(ref _startPoint, value);
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

        #region Свойство IsHintsForMove

        private bool[,] _isHintsForMove = new bool[8,8];

        public bool[,] IsHintsForMove
        {
            get => _isHintsForMove;
            set => Set(ref _isHintsForMove, value);
        }

        #endregion

        #region Свойство IsHintsForKill

        private bool[,] _isHintsForKill = new bool[8, 8];

        public bool[,] IsHintsForKill
        {
            get => _isHintsForKill;
            set => Set(ref _isHintsForKill, value);
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

    }
}
