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

        private bool IsMove(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {
            var xChange = xEnd - xStart;
            var yChange = yEnd - yStart;

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

        private bool IsKill(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {
            var xChange = xEnd - xStart;
            var yChange = yEnd - yStart;

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

        public override IEnumerable<(byte,byte)>? GetTrajectoryForMove(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {
            if (IsMove(xStart, yStart, xEnd, yEnd))
            {
                return MovePieces.GetStraightTrajectory(xStart, yStart, xEnd, yEnd);
            }
            else
            {
                return null;
            }
        }

        public override IEnumerable<(byte, byte)>? GetTrajectoryForKill(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {
            if (IsKill( xStart,  yStart,  xEnd,  yEnd))
            {
                return MovePieces.GetStraightTrajectory(xStart, yStart, xEnd, yEnd);
            }
            else
            {
                return null;
            }
        }
    }
}
