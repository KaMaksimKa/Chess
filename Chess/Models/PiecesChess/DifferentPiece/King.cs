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

        private MoveInfo? IsMove(Point startPoint, Point endPoint,Board board)
        {
            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            if (Math.Abs(xChange) <= 1 && Math.Abs(yChange) <= 1 && 
                board[endPoint.X, endPoint.Y]?.Team != Team &&
                board.CheckIsEmptySells(MovePieces.GetStraightTrajectory(startPoint, endPoint)))
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

        private MoveInfo? IsCastling(Point startPoint, Point endPoint,Board board)
        {
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
                            return new MoveInfo
                            {
                                ChangePositions = new List<ChangePosition>
                                {
                                    new ChangePosition(startPoint, endPoint),
                                    new ChangePosition(new Point(startPoint.X, 7),new Point(startPoint.X,endPoint.Y-1))
                                }
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
                            foreach (var point in trajectory)
                            {
                                var checkMoveInfo = new MoveInfo
                                {
                                    ChangePositions = new List<ChangePosition>
                                    {
                                        new ChangePosition(new Point(startPoint.X, 0), point),
                                    }
                                };
                                if (board.IsCellForKill(checkMoveInfo, point, Team))
                                {
                                    return null;
                                }
                            }

                            return new MoveInfo
                            {
                                ChangePositions = new List<ChangePosition>
                                {
                                    new ChangePosition(startPoint, endPoint),
                                    new ChangePosition(new Point(startPoint.X, 0),new Point(startPoint.X,endPoint.Y+1))
                                }
                            };
                        }
                    }
                }
            }

            return null;
        }

        public override MoveInfo? Move(Point startPoint, Point endPoint, Board board)
        {
            
            if (IsMove(startPoint, endPoint,board) is {} moveInfoIsMove)
            {
                return moveInfoIsMove;
            }
            else if (IsCastling(startPoint,  endPoint,  board) is {} moveInfoCastling)
            {
                return moveInfoCastling;
            }
            return null;
        }

        public override object Clone()
        {
            return new King(Icon, Team) { IsFirstMove = IsFirstMove };
        }
    }
}
