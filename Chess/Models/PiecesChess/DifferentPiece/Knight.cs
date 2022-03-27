
using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.PiecesChess.Base;

namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal abstract class Knight:Piece
    {
        protected Knight(string icon, TeamEnum team) : base(icon, team)
        {
        }

        private MoveInfo? IsMove(Point startPoint, Point endPoint,Board board)
        {
            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            if (((Math.Abs(xChange) == 2 && Math.Abs(yChange) == 1) ||
                (Math.Abs(xChange) == 1 && Math.Abs(yChange) == 2)) &&
                board[endPoint.X, endPoint.Y]?.Team != Team)
            {
                MoveInfo moveInfo = new MoveInfo
                {
                    ChangePositions = new List<ChangePosition> { new ChangePosition(startPoint, endPoint) }
                };
                if (board[endPoint.X, endPoint.Y] != null)
                {
                    moveInfo.KillPoint = endPoint;
                }
                return moveInfo;
            }
            
            return null;
            
        }


        public override MoveInfo? Move(Point startPoint, Point endPoint, Board board)
        {
            if (IsMove(startPoint, endPoint,board) is {} moveInfoIsMove)
            {
                return moveInfoIsMove;
            }

            return null;
        }
        
    }
}
