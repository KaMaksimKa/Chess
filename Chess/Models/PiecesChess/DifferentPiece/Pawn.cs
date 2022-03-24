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
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (xChange == 1 && yChange == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (xChange == -1 && yChange == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
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
                else
                {
                    return false;
                }
            }
            else
            {
                if (xChange == -1 && Math.Abs(yChange) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void IsEnPassant(Point startPoint, Point endPoint)
        {

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
            return moveInfo;
        }
        public override object Clone()
        {
            return new Pawn(Icon, Team,_direction) { IsFirstMove = IsFirstMove };
        }

    }
}
