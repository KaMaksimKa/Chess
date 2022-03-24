
using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.PiecesChess.Base;

namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal class Knight:Piece
    {
        public Knight(string icon, TeamEnum team) : base(icon, team)
        {
        }

        private bool IsMove(Point startPoint, Point endPoint)
        {
            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            if ((Math.Abs(xChange) == 2 && Math.Abs(yChange) == 1) ||
                (Math.Abs(xChange) == 1 && Math.Abs(yChange) == 2))
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
            if (IsMove(startPoint, endPoint) && board[endPoint.X, endPoint.Y]?.Team != Team)
            {
                moveInfo.ChangePositions = new List<ChangePosition> { new ChangePosition(startPoint, endPoint) };
                if (board[endPoint.X, endPoint.Y] != null)
                {
                    moveInfo.KillPoint = endPoint;
                }
            }

            return moveInfo;
        }
        public override object Clone()
        {
            return new Knight(Icon, Team) { IsFirstMove = IsFirstMove };
        }
    }
}
