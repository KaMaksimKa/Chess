
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;


namespace Chess.Models.Pieces.PiecesCheckers.DifferentPieces
{
    internal class Disc:Piece
    {
        public Disc(TeamEnum team,Direction direction) : base(TypePiece.Disc, team, 10)
        {
            Direction = direction;
        }
        public override Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board)
        {
            Dictionary<(Point, Point), MoveInfo> moveInfos = new Dictionary<(Point, Point), MoveInfo>();
            int direction = Direction == Direction.Up ? 1 : -1;
            List<(int, int)> moveVectors = new List<(int, int)>
            {
                (direction,1),(direction,-1),(2*direction,2),(2*direction,-2),(-2*direction,2),(-2*direction,-2)
            };

            var currentPoint = new Point();
            foreach (var (xVector, yVector) in moveVectors)
            {
                currentPoint.X = startPoint.X + xVector;
                currentPoint.Y = startPoint.Y + yVector;

                if (currentPoint.X < 0 || currentPoint.X >=board.Size.Height ||
                    currentPoint.Y < 0 || currentPoint.Y >=board.Size.Width)
                {
                    continue;
                }

                if ((xVector, yVector) == (direction, 1) || (xVector, yVector) == (direction, -1))
                {
                    if (board[currentPoint.X, currentPoint.Y] == null)
                    {
                        var moveInfo = new MoveInfo
                        {
                            Move = new ChangePosition(startPoint, currentPoint),
                            IsMoved = true,
                            ChangePositions = new[] { new ChangePosition(startPoint, currentPoint) },
                        };
                        if (currentPoint.X == 0 && Direction == Direction.Down ||
                            currentPoint.X == (board.Size.Height-1) && Direction == Direction.Up)
                        {
                            moveInfo.IsReplacePiece = true;
                            moveInfo.ReplaceImg = (currentPoint, new KingDisc(Team));
                        }
                        moveInfos.Add((startPoint, currentPoint), moveInfo);
                    }
                }
                else if ((xVector, yVector) == (2*direction, 2) || (xVector, yVector) == (2*direction, -2)||
                         (xVector, yVector) == (-2 * direction, 2) || (xVector, yVector) == (-2 * direction, -2))
                {
                    if (board[currentPoint.X, currentPoint.Y]  == null &&
                        board[currentPoint.X-xVector/2, currentPoint.Y-yVector/2] is { } piece && piece.Team!=Team) 
                    {
                        var moveInfo = new MoveInfo
                        {
                            Move = new ChangePosition(startPoint, currentPoint),
                            IsMoved = true,
                            KillPoint = new Point(currentPoint.X - xVector / 2, currentPoint.Y - yVector / 2),
                            ChangePositions = new[] { new ChangePosition(startPoint, currentPoint) }
                        };
                        if (currentPoint.X == 0 && Direction == Direction.Down ||
                            currentPoint.X == (board.Size.Height-1) && Direction == Direction.Up)
                        {
                            moveInfo.IsReplacePiece = true;
                            moveInfo.ReplaceImg = (currentPoint, new KingDisc(Team));
                        }
                        moveInfos.Add((startPoint, currentPoint), moveInfo);
                    }
                }
            }

            return moveInfos;
        }
    }
}
