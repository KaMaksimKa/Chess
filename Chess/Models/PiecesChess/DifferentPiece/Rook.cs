using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.PiecesChess.Base;

namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal class Rook:Piece
    {
        public Rook(string icon, TeamEnum team) : base(icon, team)
        {
        }

        private bool IsMove(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {
            var xChange = xEnd - xStart;
            var yChange = yEnd - yStart;
            
            if ((xChange == 0 && yChange != 0 ) || (xChange != 0 && yChange == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsKill(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {
            return IsMove(xStart, yStart, xEnd, yEnd);
        }

        public override IEnumerable<(byte,byte)>? GetTrajectoryForMove(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {
            if (IsMove(xStart, yStart, xEnd, yEnd))
                return MovePieces.GetStraightTrajectory(xStart, yStart, xEnd, yEnd);
            else
                return null;
        }

        public override IEnumerable<(byte, byte)>? GetTrajectoryForKill(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {
            if (IsKill( xStart,  yStart,  xEnd,  yEnd))
                return MovePieces.GetStraightTrajectory(xStart, yStart, xEnd, yEnd);
            else
                return null;
        }
    }
}
