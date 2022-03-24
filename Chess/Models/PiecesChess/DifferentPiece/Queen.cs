using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.PiecesChess.Base;

namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal class Queen:Piece
    {
        public Queen(string icon, TeamEnum team) : base(icon, team)
        {
        }

        private bool IsMove(Point startPoint, Point endPoint)
        {
            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            if ((xChange == 0 && yChange != 0 ) ||
                (xChange != 0 && yChange == 0) ||
                (Math.Abs(xChange) == Math.Abs(yChange)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override MoveInfo Move(Point startPoint, Point endPoint, Board board)
        {
            MoveInfo moveInfo = new MoveInfo();
            if (IsMove(startPoint, endPoint) && board[endPoint.X, endPoint.Y]?.Team != Team &&
                board.CheckIsEmptySells(MovePieces.GetStraightTrajectory(startPoint, endPoint)))
            {
                 moveInfo.ChangePositions = new List<ChangePosition> { new ChangePosition(startPoint, endPoint) } ;
                if (board[endPoint.X, endPoint.Y] != null)
                {
                    moveInfo.KillPoint = endPoint;
                }
            }
            return moveInfo;
        }
        public override object Clone()
        {
            return new Queen(Icon, Team) { IsFirstMove = IsFirstMove };
        }
    }
}
