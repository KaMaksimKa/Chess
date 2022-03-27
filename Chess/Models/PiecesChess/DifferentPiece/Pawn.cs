using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.PiecesChess.Base;

namespace Chess.Models.PiecesChess.DifferentPiece
{
    enum PawnDirection
    {
        Up,
        Down
    }
    internal abstract class Pawn:Piece
    {
        protected readonly PawnDirection Direction;

        protected Pawn(string icon, TeamEnum team, PawnDirection direction) : base(icon, team)
        {
            Direction = direction;
        }

        private MoveInfo? IsMove(Point startPoint, Point endPoint,Board board)
        {
            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            if (Direction == PawnDirection.Up)
            {
                if (IsFirstMove)
                {
                    if (!(xChange == 1 || (xChange == 2)) || yChange != 0)
                    {
                        return null;
                    }
                }
                else
                {
                    if (xChange != 1 || yChange != 0)
                    {
                        return null;
                    }
                }
            }
            else
            {
                if (IsFirstMove)
                {
                    if (!(xChange == -1 || (xChange == -2)) || yChange != 0)
                    {
                        return null;
                    }
                }
                else
                {
                    if (xChange != -1 || yChange != 0)
                    {
                        return null;
                    }
                }
            }

            if (board[endPoint.X, endPoint.Y] is null &&
                board.CheckIsEmptySells(MovePieces.GetStraightTrajectory(startPoint, endPoint)))
            {
                MoveInfo moveInfo = new MoveInfo
                {
                    ChangePositions = new List<ChangePosition> { new ChangePosition(startPoint, endPoint) }
                };
                return moveInfo;
            }
            return null;
        }

        private MoveInfo? IsKill(Point startPoint, Point endPoint,Board board)
        {
            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            if (Direction == PawnDirection.Up)
            {
                if (xChange != 1 || Math.Abs(yChange) != 1)
                {
                    return null;
                }
            }
            else
            {
                if (xChange != -1 || Math.Abs(yChange) != 1)
                {
                    return null;
                }
            }

            if (board[endPoint.X, endPoint.Y] is { } piece &&
                piece.Team != Team &&
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

        private MoveInfo EnPassant(Point startPoint, Point endPoint, Board board)
        {
            MoveInfo moveInfo = new MoveInfo();

            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            if (Direction == PawnDirection.Up)
            {
                if (xChange == 1 && Math.Abs(yChange) == 1 &&
                    board.LastMoveInfo.ChangePositions is {} changePositions)
                {
                    foreach (var (startP,endP) in changePositions)
                    {
                        if (startP.X == endP.X+2 && startP.Y == endPoint.Y &&
                            endP.X == startPoint.X && endP.Y == endPoint.Y)
                        {
                            moveInfo.ChangePositions = new List<ChangePosition>
                            {
                                new ChangePosition(startPoint, endPoint)
                            };
                            moveInfo.KillPoint = new Point(startPoint.X,endPoint.Y);
                        }
                    }
                    

                }
            }
            else
            {
                if (xChange == -1 && Math.Abs(yChange) == 1 &&
                    board.LastMoveInfo.ChangePositions is { } changePositions)
                {
                    foreach (var (startP, endP) in changePositions)
                    {
                        if (startP.X == endP.X - 2 && startP.Y == endPoint.Y &&
                            endP.X == startPoint.X && endP.Y == endPoint.Y)
                        {
                            moveInfo.ChangePositions = new List<ChangePosition>
                            {
                                new ChangePosition(startPoint, endPoint)
                            };
                            moveInfo.KillPoint = new Point(startPoint.X, endPoint.Y);
                        }
                    }
                }
            }

            return moveInfo;
        }
        public override MoveInfo? Move(Point startPoint, Point endPoint, Board board)
        {
            if (IsMove(startPoint, endPoint,board) is {} moveInfoIsMove)
            {
                return moveInfoIsMove;
            }
            else if (IsKill(startPoint, endPoint ,board) is {} moveInfoIsKill)
            {
                return moveInfoIsKill;
            }
            else if (EnPassant(startPoint,endPoint,board) is {ChangePositions:{}} moveInfoEnPassant)
            {
                return moveInfoEnPassant;
            }

            return null;
        }
        

    }
}
