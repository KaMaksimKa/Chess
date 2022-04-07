using System;
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

            var currPoint = new Point();
            foreach (var (xVector, yVector) in moveVectors)
            {
                currPoint.X = startPoint.X + xVector;
                currPoint.Y = startPoint.Y + yVector;
                if (currPoint.X is < 0 or > 7 || currPoint.Y is < 0 or > 7)
                {
                    continue;
                }

                if ((xVector, yVector) == (direction, 1) || (xVector, yVector) == (direction, -1))
                {
                    if (board[currPoint.X, currPoint.Y] == null)
                    {
                        var moveInfo = new MoveInfo
                        {
                            Move = new ChangePosition(startPoint, currPoint),
                            IsMoved = true,
                            ChangePositions = new[] { new ChangePosition(startPoint, currPoint) },
                        };
                        if (currPoint.X == 0 && Direction == Direction.Down ||
                            currPoint.X == 7 && Direction == Direction.Up)
                        {
                            moveInfo.IsReplacePiece = true;
                            moveInfo.ReplaceImg = (currPoint, new KingDisc(Team));
                        }
                        moveInfos.Add((startPoint, currPoint), moveInfo);
                    }
                }
                else if ((xVector, yVector) == (2*direction, 2) || (xVector, yVector) == (2*direction, -2)||
                         (xVector, yVector) == (-2 * direction, 2) || (xVector, yVector) == (-2 * direction, -2))
                {
                    if (board[currPoint.X, currPoint.Y]  == null &&
                        board[currPoint.X-xVector/2, currPoint.Y-yVector/2] is { } piece && piece.Team!=Team) 
                    {
                        var moveInfo = new MoveInfo
                        {
                            Move = new ChangePosition(startPoint, currPoint),
                            IsMoved = true,
                            KillPoint = new Point(currPoint.X - xVector / 2, currPoint.Y - yVector / 2),
                            ChangePositions = new[] { new ChangePosition(startPoint, currPoint) }
                        };
                        if (currPoint.X == 0 && Direction == Direction.Down ||
                            currPoint.X == 7 && Direction == Direction.Up)
                        {
                            moveInfo.IsReplacePiece = true;
                            moveInfo.ReplaceImg = (currPoint, new KingDisc(Team));
                        }
                        moveInfos.Add((startPoint, currPoint), moveInfo);
                    }
                }
            }

            return moveInfos;
        }
    }
}
