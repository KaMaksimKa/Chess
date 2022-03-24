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
    internal class Pawn:Piece
    {
        private readonly PawnDirection _direction;
        public Pawn(string icon, TeamEnum team, PawnDirection direction) : base(icon, team)
        {
            _direction = direction;
        }

        private bool IsMove(Point startPoint, Point endPoint)
        {
            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            if (_direction == PawnDirection.Up)
            {
                if (IsFirstMove)
                {
                    if ((xChange == 1 || (xChange == 2)) && yChange == 0)
                    {
                        return true;
                    }
                }
                else
                {
                    if (xChange == 1 && yChange == 0)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (IsFirstMove)
                {
                    if ((xChange == -1 || (xChange == -2)) && yChange == 0)
                    {
                        return true;
                    }
                }
                else
                {
                    if (xChange == -1 && yChange == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsKill(Point startPoint, Point endPoint)
        {
            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            if (_direction == PawnDirection.Up)
            {
                if (xChange == 1 && Math.Abs(yChange) == 1)
                {
                    return true;
                }
            }
            else
            {
                if (xChange == -1 && Math.Abs(yChange) == 1)
                {
                    return true;
                }
            }
            return false;
        }

        private MoveInfo EnPassant(Point startPoint, Point endPoint, Board board)
        {
            MoveInfo moveInfo = new MoveInfo();

            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            if (_direction == PawnDirection.Up)
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
        public override MoveInfo Move(Point startPoint, Point endPoint, Board board)
        {
            MoveInfo moveInfo = new MoveInfo();
            if (IsMove(startPoint, endPoint) &&
                board[endPoint.X, endPoint.Y] is null &&
                board.CheckIsEmptySells(MovePieces.GetStraightTrajectory(startPoint, endPoint)))
            {
                moveInfo.ChangePositions = new List<ChangePosition> { new ChangePosition(startPoint, endPoint)  };
            }
            else if (IsKill(startPoint, endPoint) &&
                     board[endPoint.X, endPoint.Y] is {} piece &&
                     piece.Team != Team &&
                     board.CheckIsEmptySells(MovePieces.GetStraightTrajectory(startPoint, endPoint)))
            {
                moveInfo.ChangePositions = new List<ChangePosition> { new ChangePosition(startPoint, endPoint) };
                if (board[endPoint.X, endPoint.Y] != null)
                {
                    moveInfo.KillPoint = endPoint;
                }
            }
            else if (EnPassant(startPoint,endPoint,board) is {ChangePositions:{}} moveInfoEnPassant)
            {
                return moveInfoEnPassant;
            }
            return moveInfo;
        }
        public override object Clone()
        {
            return new Pawn(Icon, Team,_direction) { IsFirstMove = IsFirstMove };
        }

    }
}
