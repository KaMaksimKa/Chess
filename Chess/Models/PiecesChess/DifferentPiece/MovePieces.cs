using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;


namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal static class MovePieces
    {
        public static Dictionary<(Point,Point), MoveInfo> GetStraightMoves(List<(short, short)> moveVectors,Point startPoint,Board board,TeamEnum team)
        {
            Dictionary<(Point, Point), MoveInfo> moveInfos = new Dictionary<(Point, Point), MoveInfo>();

            foreach (var (xVector,yVector) in moveVectors)
            {
                var currentPoint = startPoint;
                while (true)
                {
                    currentPoint.X += xVector;
                    currentPoint.Y += yVector;
                    if (currentPoint.X is < 0 or > 7 || currentPoint.Y is < 0 or > 7)
                    {
                        break;
                    }
                    if (board[currentPoint.X, currentPoint.Y]?.Team != team)
                    {
                        var moveInfo = new MoveInfo {ChangePositions = new[]{new ChangePosition
                            {
                                StartPoint = startPoint,
                                EndPoint = currentPoint
                            }}};
                        if (board[currentPoint.X, currentPoint.Y] is { })
                        {
                            moveInfo.KillPoint = currentPoint;
                            moveInfos.Add((startPoint, currentPoint), moveInfo);
                            break;
                        }
                        moveInfos.Add((startPoint,currentPoint),moveInfo);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return moveInfos;
        }
        public static List<Point> GetStraightTrajectory(Point startPoint, Point endPoint)
        {
            List<Point> straightTrajectory = new List<Point>();

            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;
            if (xChange == 0 && yChange == 0)
            {
                return straightTrajectory;
            }
            var xChangeStep = xChange / Math.Max(Math.Abs(xChange), Math.Abs(yChange));
            var yChangeStep = yChange / Math.Max(Math.Abs(xChange), Math.Abs(yChange));

            int currentX = startPoint.X;
            int currentY = startPoint.Y;

            while (!((currentX + xChangeStep) == endPoint.X && (currentY + yChangeStep) == endPoint.Y))
            {
                currentX += xChangeStep;
                currentY += yChangeStep;
                straightTrajectory.Add(new Point(currentX, currentY));
            }

            return straightTrajectory;
        }
        
    }
}
