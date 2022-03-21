using System.Drawing;
using ChessConsole.Models.PiecesChess.Base;

namespace ChessConsole.Models.PiecesChess.DifferentPiece
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

        private bool IsMove(Point pos, Point newPos)
        {
            Point changePos = new(newPos.X - pos.X, newPos.Y - pos.Y);
            if (_direction == PawnDirection.Up)
            {
                if (IsFirstMove)
                {
                    if ((changePos.X == 1 || (changePos.X == 2)) && changePos.Y == 0)
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
                    if (changePos.X == 1 && changePos.Y == 0)
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
                    if ((changePos.X == -1 || (changePos.X == -2)) && changePos.Y == 0)
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
                    if (changePos.X == -1 && changePos.Y == 0)
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

        private bool IsKill(Point pos, Point newPos)
        {
            Point changePos = new(newPos.X - pos.X, newPos.Y - pos.Y);
            if (_direction == PawnDirection.Up)
            {
                if (changePos.X == 1 && Math.Abs(changePos.Y) == 1)
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
                if (changePos.X == -1 && Math.Abs(changePos.Y) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override IEnumerable<Point> GetTrajectoryForMove(Point pos, Point newPos)
        {
            if (IsMove(pos, newPos))
            {
                return MovePieces.GetStraightTrajectory(pos, newPos);
            }
            else
            {
                throw new ApplicationException("Сюда нельзя походить");
            }
        }

        public override IEnumerable<Point> GetTrajectoryForKill(Point pos, Point newPos)
        {
            if (IsKill(pos, newPos))
            {
                return MovePieces.GetStraightTrajectory(pos, newPos);
            }
            else
            {
                throw new ApplicationException("Сюда нельзя походить");
            }
        }
    }
}
