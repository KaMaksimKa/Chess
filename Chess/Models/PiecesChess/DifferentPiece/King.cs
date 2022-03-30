
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;
using Chess.Models.PiecesChess.Base;

namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal abstract class King:Piece
    {
        protected King(string icon, TeamEnum team) : base(icon, team,900)
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
                        ChangePositions = new List<ChangePosition>
                        {
                            new ChangePosition(startPoint, new Point(startPoint.X,2)),
                            new ChangePosition(new Point(startPoint.X, 0),new Point(startPoint.X,3))
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
                        ChangePositions = new List<ChangePosition>
                        {
                            new ChangePosition(startPoint, new Point(startPoint.X,6)),
                            new ChangePosition(new Point(startPoint.X, 7),new Point(startPoint.X,5))
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
                if (currPoint.X is < 0 or > 7 || currPoint.Y is < 0 or > 7)
                {
                    continue;
                }
                if (board[currPoint.X, currPoint.Y]?.Team != Team)
                {
                    var moveInfo = new MoveInfo
                    {
                        ChangePositions = new[]{new ChangePosition
                        {
                            StartPoint = startPoint,
                            EndPoint = currPoint
                        }}
                    };
                    if (board[currPoint.X, currPoint.Y] is { })
                    {
                        moveInfo.KillPoint = currPoint;
                    }
                    moveInfos.Add((startPoint, currPoint), moveInfo);
                }
            }

            if (IsCastlingLeft(startPoint, board) is { } moveInfoLeft)
            {
                moveInfos.Add((startPoint,new Point(startPoint.X,2)),moveInfoLeft);
            }
            if (IsCastlingRight(startPoint, board) is { } moveInfoRight)
            {
                moveInfos.Add((startPoint, new Point(startPoint.X, 6)), moveInfoRight);
            }
            return moveInfos;
        }

    }
}
