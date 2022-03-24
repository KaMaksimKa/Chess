using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.PiecesChess.Base;

namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal class King:Piece
    {
        public King(string icon, TeamEnum team) : base(icon, team)
        {
        }

        private bool IsMove(Point startPoint, Point endPoint)
        {
            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            if (Math.Abs(xChange) <= 1 && Math.Abs(yChange) <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private MoveInfo Castling(Point startPoint, Point endPoint,Board board)
        {
            //Доделать
            MoveInfo moveInfo = new MoveInfo();

            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;
            if (xChange == 0)
            {
                if (yChange == 2)
                {
                    if (board[startPoint.X, 7] is Rook { IsFirstMove: true })
                    {
                        var trajectory = MovePieces.GetStraightTrajectory(startPoint, new Point(startPoint.X, 7));
                        if (board.CheckIsEmptySells(trajectory))
                        {
                            moveInfo.ChangePositions = new List<ChangePosition>
                            {
                                new ChangePosition(startPoint, endPoint),
                                new ChangePosition(new Point(startPoint.X, 7),new Point(startPoint.X,endPoint.Y-1))
                            };

                        }
                    }
                }
                else if (yChange == -2)
                {
                    if (board[startPoint.X, 0] is Rook { IsFirstMove: true })
                    {
                        var trajectory = MovePieces.GetStraightTrajectory(startPoint, new Point(startPoint.X, 0));
                        if (board.CheckIsEmptySells(trajectory))
                        {
                            moveInfo.ChangePositions = new List<ChangePosition>
                            {
                                new ChangePosition(startPoint, endPoint),
                                new ChangePosition(new Point(startPoint.X, 0),new Point(startPoint.X,endPoint.Y+1))
                            };
                        }
                    }
                }
            }

            return moveInfo;
        }



        public override MoveInfo Move(Point startPoint, Point endPoint, Board board)
        {
            MoveInfo moveInfo = new MoveInfo();
            if (IsMove(startPoint, endPoint) && board[endPoint.X, endPoint.Y]?.Team != Team &&
                board.CheckIsEmptySells(MovePieces.GetStraightTrajectory(startPoint, endPoint)))
            {
                moveInfo.ChangePositions = new List<ChangePosition> { new ChangePosition(startPoint, endPoint) };
                if (board[endPoint.X, endPoint.Y] != null)
                {
                    moveInfo.KillPoint = endPoint;
                }
            }
            else if (Castling(startPoint,  endPoint,  board) is {ChangePositions:{}} moveInfoCastling)
            {
                return moveInfoCastling;
            }
            {
                
            }
            return moveInfo;
        }
    }
}
