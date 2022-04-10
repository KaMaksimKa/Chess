
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;


namespace Chess.Models.Pieces.PiecesChess.DifferentPiece
{
    internal class King:Piece
    {
        public King(TeamEnum team) : base(TypePiece.King, team,900,
            new[,]{
                { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},
                {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},
                {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},
                {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},
                {-2.0, -3.0, -3.0, -4.0, -4.0, -3.0, -3.0, -2.0},
                {-1.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -1.0},
                {2.0, 2.0, 0.0, 0.0, 0.0, 0.0, 2.0, 2.0},
                {2.0, 3.0, 1.0, 0.0, 0.0, 1.0, 3.0, 2.0}
            })
        {
        }

        protected virtual MoveInfo? IsCastlingLeft(Point startPoint,Board board)
        {
            if (IsFirstMove && board[startPoint.X,0] is Rook{IsFirstMove:true})
            {
                var trajectory = MovePieces.GetStraightTrajectory(startPoint, new Point(startPoint.X, 0));
                if (board.CheckIsEmptySells(trajectory))
                {
                    /*foreach (var point in trajectory)
                    {
                        var checkMoveInfo = new MoveInfo
                        {
                            ChangePositions = new List<ChangePosition>
                            {
                                new ChangePosition(new Point(startPoint.X, 0), point),
                            }
                        };
                        if (board.IsCellForKill(checkMoveInfo, point, Team))
                        {
                            return null;
                        }
                    }*/

                    return new MoveInfo
                    {
                        Move = new ChangePosition(startPoint, new Point(startPoint.X, startPoint.Y - 2)),
                        IsMoved = true,
                        ChangePositions = new List<ChangePosition>
                        {
                            new ChangePosition(startPoint, new Point(startPoint.X,startPoint.Y-2)),
                            new ChangePosition(new Point(startPoint.X, 0),new Point(startPoint.X,startPoint.Y-1))
                        }
                    };
                }
            }

            return null;
        }

        protected virtual MoveInfo? IsCastlingRight(Point startPoint, Board board)
        {
            if (IsFirstMove && board[startPoint.X, 7] is Rook { IsFirstMove: true })
            {
                var trajectory = MovePieces.GetStraightTrajectory(startPoint, new Point(startPoint.X, 7));
                if (board.CheckIsEmptySells(trajectory))
                {
                    /*foreach (var point in trajectory)
                    {
                        var checkMoveInfo = new MoveInfo
                        {
                            ChangePositions = new List<ChangePosition>
                            {
                                new ChangePosition(new Point(startPoint.X, 7), point),
                            }
                        };
                        if (board.IsCellForKill(checkMoveInfo, point, Team))
                        {
                            return null;
                        }
                    }*/

                    return new MoveInfo
                    {
                        Move = new ChangePosition(startPoint, new Point(startPoint.X, startPoint.Y + 2)),
                        IsMoved = true,
                        ChangePositions = new List<ChangePosition>
                        {
                            new ChangePosition(startPoint, new Point(startPoint.X,startPoint.Y+2)),
                            new ChangePosition(new Point(startPoint.X, 7),new Point(startPoint.X,startPoint.Y+1))
                        }
                    };
                }
            }

            return null;
        }

        public override Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board)
        {
            Dictionary<(Point, Point), MoveInfo> moveInfos = new Dictionary<(Point, Point), MoveInfo>();

            List<(int, int)> moveVectors = new List<(int, int)>
            {
                (1,0),(1,1),(0,1),(-1,0),(-1,-1),(0,-1),(1,-1),(-1,1)
            };

            var currPoint = new Point();
            foreach (var (xVector, yVector) in moveVectors)
            {
                currPoint.X = startPoint.X + xVector;
                currPoint.Y = startPoint.Y + yVector;
                if (currPoint.X < 0 || currPoint.X >= board.Size.Height ||
                    currPoint.Y < 0 || currPoint.Y >= board.Size.Width)
                {
                    continue;
                }
                if (board[currPoint.X, currPoint.Y]?.Team != Team)
                {
                    var moveInfo = new MoveInfo
                    {
                        Move = new ChangePosition(startPoint, currPoint),
                        IsMoved = true,
                        ChangePositions = new[]{ new ChangePosition(startPoint,currPoint)}
                    };
                    if (board[currPoint.X, currPoint.Y] is { })
                    {
                        moveInfo.KillPoint = currPoint;
                    }
                    moveInfos.TryAdd((startPoint, currPoint), moveInfo);
                }
            }

            if (IsCastlingLeft(startPoint, board) is { } moveInfoLeft)
            {
                moveInfos.TryAdd((startPoint, new Point(startPoint.X, startPoint.Y-2)), moveInfoLeft);
            }
            if (IsCastlingRight(startPoint, board) is { } moveInfoRight)
            {
                moveInfos.TryAdd((startPoint, new Point(startPoint.X, startPoint.Y +2)), moveInfoRight);
            }
            return moveInfos;
        }

    }
}
