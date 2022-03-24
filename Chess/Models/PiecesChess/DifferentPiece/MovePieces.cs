using System;
using System.Collections.Generic;
using System.Drawing;


namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal static class MovePieces
    {
        public static IEnumerable<(byte, byte)> GetStraightTrajectory(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {
            var xChange = xEnd - xStart;
            var yChange = yEnd - yStart;

            var xChangeStep = xChange / Math.Max(Math.Abs(xChange), Math.Abs(yChange));
            var yChangeStep = yChange / Math.Max(Math.Abs(xChange), Math.Abs(yChange));

            int currentX = xStart;
            int currentY = yStart;

            while (!((currentX + xChangeStep) == xEnd && (currentY + yChangeStep) == yEnd))
            {
                currentX += xChangeStep;
                currentY += yChangeStep;
                yield return ((byte) currentX, (byte) currentY);
            }
        }
        
    }
}
