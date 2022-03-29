
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.PiecesChess.Base;

namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal abstract class Knight:Piece
    {
        protected Knight(string icon, TeamEnum team) : base(icon, team)
        {
        }

        public override Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board)
        {
            List<(short, short)> moveVectors = new List<(short, short)>
            {
                (2, 1), (2, -1), (-2, 1), (-2, -1),(1, 2), (1, -2), (-1, 2), (-1, -2)
            };

            Dictionary<(Point, Point), MoveInfo> moveInfos = new Dictionary<(Point, Point), MoveInfo>();

            var currentPoint = new Point();

            foreach (var (xVector,yVector) in moveVectors)
            {
                currentPoint.X = startPoint.X + xVector;
                currentPoint.Y = startPoint.Y + yVector;
                if (currentPoint.X is < 0 or > 7 || currentPoint.Y is < 0 or > 7)
                {
                    continue;
                }
                if (board[currentPoint.X, currentPoint.Y]?.Team != Team)
                {
                    var moveInfo = new MoveInfo
                    {
                        ChangePositions = new[]{new ChangePosition
                        {
                            StartPoint = startPoint,
                            EndPoint = currentPoint
                        }}
                    };
                    if (board[currentPoint.X, currentPoint.Y] is { })
                    {
                        moveInfo.KillPoint = currentPoint;
                    }
                    moveInfos.Add((startPoint,currentPoint),moveInfo);
                }
            }

            return moveInfos;
        }

        
        
    }
}
