using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;

namespace Chess.Models.Pieces.PiecesCheckers.DifferentPieces
{
    internal class KingDisc:Piece
    {
        public KingDisc(TeamEnum team) : base(TypePiece.KingDisc, team, 30)
        {
        }

        public override Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board)
        {
            Dictionary<(Point, Point), MoveInfo> moveInfos = new Dictionary<(Point, Point), MoveInfo>();
            List<(short, short)> moveVectors = new List<(short, short)> { (1, 1), (-1, 1), (1, -1), (-1, -1) };

            foreach (var (xVector,yVector) in moveVectors)
            {
                var currentPoint = startPoint;
                Point? killPoint = null;
                while (true)
                {
                    currentPoint.X += xVector;
                    currentPoint.Y += yVector;
                    if (currentPoint.X is < 0 or > 7 || currentPoint.Y is < 0 or > 7)
                    {
                        break;
                    }

                    if (killPoint == null)
                    {
                        if (board[currentPoint.X, currentPoint.Y] == null)
                        {
                            
                            moveInfos.Add((startPoint, currentPoint), new MoveInfo
                            {
                                IsMoved = true,
                                ChangePositions = new[] { new ChangePosition(startPoint, currentPoint) },
                                Move = new ChangePosition(startPoint, currentPoint)
                            });
                            
                        }
                        else if (board[currentPoint.X, currentPoint.Y] is { } piece && piece.Team != Team)
                        {
                            var endPoint = new Point(currentPoint.X + xVector, currentPoint.Y + yVector);
                            if (endPoint is {X:>=0 and <=7,Y: >= 0 and <= 7 } && board[endPoint.X, endPoint.Y] == null)
                            {
                                killPoint = currentPoint;
                                moveInfos.Add((startPoint, endPoint), new MoveInfo
                                {
                                    IsMoved = true,
                                    ChangePositions = new[] { new ChangePosition(startPoint, endPoint) },
                                    Move = new ChangePosition(startPoint, endPoint),
                                    KillPoint = currentPoint
                                });
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (board[currentPoint.X, currentPoint.Y] == null)
                        {
                            if (currentPoint != new Point(killPoint.Value.X+xVector,killPoint.Value.Y+yVector) )
                            {
                                moveInfos.Add((startPoint, currentPoint), new MoveInfo
                                {
                                    IsMoved = true,
                                    ChangePositions = new[] { new ChangePosition(startPoint, currentPoint) },
                                    Move = new ChangePosition(startPoint, currentPoint),
                                    KillPoint = killPoint
                                });
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                }
            }
            return moveInfos;
        }
    }
}
