using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;


namespace Chess.Models.Pieces.PiecesChess.DifferentPiece
{
    internal class Pawn:Piece
    {
        public readonly Direction Direction;

        public Pawn(TeamEnum team, Direction direction) : base(TypePiece.Pawn, team,10,
            new [,] {{0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0}, {5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0}, {1.0, 1.0, 2.0, 3.0, 3.0, 2.0, 1.0, 1.0}, {0.5, 0.5, 1.0, 2.5, 2.5, 1.0, 0.5, 0.5}, {0.0, 0.0, 0.0, 2.0, 2.0, 0.0, 0.0, 0.0}, {0.5, -0.5, -1.0, 0.0, 0.0, -1.0, -0.5, 0.5}, {0.5, 1.0, 1.0, -2.0, -2.0, 1.0, 1.0, 0.5}, {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0}})
        {
            Direction = direction;
            ReplacementPieces = new List<Piece>
            {
                new Queen(team),new Knight(team),new Bishop(team),
                new Rook(team)
            };
        }

        private MoveInfo? EnPassant(Point startPoint, Point endPoint, Board board)
        {
            

            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            int direction = Direction == Direction.Up ? 1 : -1;

            if (xChange == direction && Math.Abs(yChange) == 1 &&
                board[startPoint.X,startPoint.Y+yChange] is Pawn &&
                board.LastMoveInfo.ChangePositions is {} changePositions)
            {
                foreach (var (startP,endP) in changePositions)
                {
                    if (startP.X == endP.X+2*direction && startP.Y == endPoint.Y &&
                        endP.X == startPoint.X && endP.Y == endPoint.Y)
                    {
                        return new MoveInfo()
                        {
                            Move = new ChangePosition(startPoint, endPoint),
                            IsMoved = true,
                            KillPoint = new Point(startPoint.X, endPoint.Y),
                            ChangePositions = new List<ChangePosition>
                            {
                                new ChangePosition(startPoint, endPoint)
                            }
                        };
                    }
                }
            }

            return null;
    }

        public new List<Piece> ReplacementPieces { get; init; }

        public override Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board)
        {
            Dictionary<(Point, Point), MoveInfo> moveInfos = new Dictionary<(Point, Point), MoveInfo>();

            int direction = Direction == Direction.Up ? 1 : -1;

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
                    var moveInfo = new MoveInfo
                    {
                        Move = new ChangePosition(startPoint, currPoint),
                        IsMoved = true,
                        ChangePositions = new[] {new ChangePosition(startPoint, currPoint)},
                    };
                    if (currPoint.X is 0 or 7)
                    {
                        moveInfo.IsReplacePiece = true;
                        moveInfo.ReplaceImg = (currPoint, null);
                    }
                    moveInfos.Add((startPoint, currPoint), moveInfo);
                }
                else if ((xVector, yVector) == (2*direction, 0) && board[currPoint.X,currPoint.Y] == null &&
                         board[currPoint.X-direction, currPoint.Y] == null && IsFirstMove)
                {
                    moveInfos.Add((startPoint, currPoint), new MoveInfo
                    {
                        Move = new ChangePosition(startPoint, currPoint),
                        IsMoved = true,
                        ChangePositions = new[] {new ChangePosition(startPoint,currPoint)}
                    });
                }
                else if ((xVector, yVector) == (direction, 1) || (xVector, yVector) == (direction, -1))
                {
                    if (board[currPoint.X, currPoint.Y] is { } piece && piece.Team!=Team)
                    {
                        var moveInfo = new MoveInfo
                        {
                            Move = new ChangePosition(startPoint, currPoint),
                            IsMoved = true,
                            KillPoint = currPoint,
                            ChangePositions = new[] { new ChangePosition(startPoint, currPoint) }
                        };
                        if (currPoint.X is 0 or 7)
                        {
                            moveInfo.IsReplacePiece = true;
                            moveInfo.ReplaceImg = (currPoint, null);
                        }
                        moveInfos.Add((startPoint, currPoint), moveInfo);
                    }
                    else if (EnPassant(startPoint,currPoint,board) is {} moveInfoEnPassant)
                    {
                        moveInfos.Add((startPoint, currPoint), moveInfoEnPassant);
                    }
                }

            }

            return moveInfos;
        }

    }
}
