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
    internal abstract class Pawn:Piece
    {
        public readonly PawnDirection Direction;

        protected Pawn(string icon, TeamEnum team, PawnDirection direction) : base(icon, team)
        {
            Direction = direction;
        }

        private MoveInfo EnPassant(Point startPoint, Point endPoint, Board board)
        {
            MoveInfo moveInfo = new MoveInfo();

            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            int direction = Direction == PawnDirection.Up ? 1 : -1;

            if (xChange == direction && Math.Abs(yChange) == 1 &&
                board[startPoint.X,startPoint.Y+yChange] is Pawn &&
                board.LastMoveInfo.ChangePositions is {} changePositions)
            {
                foreach (var (startP,endP) in changePositions)
                {
                    if (startP.X == endP.X+2*direction && startP.Y == endPoint.Y &&
                        endP.X == startPoint.X && endP.Y == endPoint.Y)
                    {
                        moveInfo.ChangePositions = new List<ChangePosition>
                        {
                            new ChangePosition(startPoint, endPoint)
                        };
                        moveInfo.KillPoint = new Point(startPoint.X,endPoint.Y);
                    }
                }
            }

            return moveInfo;
    }

        public override Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board)
        {
            Dictionary<(Point, Point), MoveInfo> moveInfos = new Dictionary<(Point, Point), MoveInfo>();

            int direction = Direction == PawnDirection.Up ? 1 : -1;

            List<(int, int)> moveVectors = new List<(int, int)>
            {
                (direction,0),(2*direction,0),(direction,1),(direction,-1)
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

                if ((xVector, yVector) == (direction, 0) && board[currPoint.X,currPoint.Y] == null)
                {
                    moveInfos.Add((startPoint, currPoint), new MoveInfo
                    {
                        ChangePositions = new[]
                        {
                            new ChangePosition(startPoint,currPoint)
                        }
                    });
                }
                else if ((xVector, yVector) == (2*direction, 0) && board[currPoint.X,currPoint.Y] == null &&
                         board[currPoint.X-direction, currPoint.Y] == null && IsFirstMove)
                {
                    moveInfos.Add((startPoint, currPoint), new MoveInfo
                    {
                        ChangePositions = new[]
                        {
                            new ChangePosition(startPoint,currPoint)
                        }
                    });
                }
                else if ((xVector, yVector) == (direction, 1) || (xVector, yVector) == (direction, -1))
                {
                    if (board[currPoint.X, currPoint.Y] is { } piece && piece.Team!=Team)
                    {
                        moveInfos.Add((startPoint, currPoint), new MoveInfo
                        {
                            KillPoint = currPoint,
                            ChangePositions = new[]
                            {
                                new ChangePosition(startPoint,currPoint)
                            }
                        });
                    }
                    else if (EnPassant(startPoint,currPoint,board) is {ChangePositions:{}} moveInfoEnPassant)
                    {
                        moveInfos.Add((startPoint, currPoint), moveInfoEnPassant);
                    }
                }

            }

            return moveInfos;
        }

    }
}
