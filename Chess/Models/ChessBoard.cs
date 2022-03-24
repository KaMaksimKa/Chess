using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.PiecesChess;
using Chess.Models.PiecesChess.Base;
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models
{
    internal class ChessBoard:Board
    {
        public TeamEnum WhoseMove { get; set; } = TeamEnum.WhiteTeam;

        /*public bool IsCastling(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {
            if (ArrayBoard[xStart, yStart] is King {IsFirstMove: true} &&
                ArrayBoard[xStart, yStart]?.Team == WhoseMove)
            {
                var xChange = xEnd - xStart;
                var yChange = yEnd - yStart;

                if (xChange == 0)
                {
                    if (yChange == 2)
                    {
                        if (ArrayBoard[xStart, 7] is Rook {IsFirstMove: true})
                        {
                            var trajectory = MovePieces.GetStraightTrajectory(xStart, yStart, xStart, 7);
                            return CheckIsEmptySells(trajectory);
                        }
                    }
                    else if (yChange == -2)
                    {
                        if (ArrayBoard[xStart, 0] is Rook { IsFirstMove: true })
                        {
                            var trajectory = MovePieces.GetStraightTrajectory(xStart, yStart, xStart, 0);
                            return CheckIsEmptySells(trajectory);
                        }
                    }
                }
            }
            return false;
        }*/

        public HintsChess GetHintsForPiece(Point startPoint)
        {
            bool[,] hintsForMove = new bool[8, 8];
            bool[,] hintsForKill = new bool[8, 8];
            if (ArrayBoard[startPoint.X, startPoint.Y] is { } piece && piece.Team == WhoseMove)
            {
                for (byte i = 0; i < 8; i++)
                {
                    for (byte j = 0; j < 8; j++)
                    {
                        MoveInfo moveInfo = piece.Move(startPoint,new Point(i,j),this);

                        if (moveInfo.KillPoint != null)
                        {
                            hintsForKill[i,j] = true;
                        }
                        else if (moveInfo.ChangePositions != null)
                        {
                            hintsForMove[i,j] = true;
                        }
                    }
                }
            }

            return new HintsChess {IsHintsForKill = hintsForKill, IsHintsForMove = hintsForMove};
        }

        public MoveInfo Move(Point startPoint, Point endPoint)
        {
            if (startPoint.X is > 0 and < 7 && startPoint.Y is > 0 and < 7 &&
                endPoint.X is > 0 and < 7 && endPoint.Y is > 0 and < 7)
            {
                if (ArrayBoard[startPoint.X, startPoint.Y] is { } piece && piece.Team == WhoseMove)
                {
                    MoveInfo moveInfo = piece.Move(startPoint, endPoint, this);

                    if (moveInfo.KillPoint is { } killPoint)
                    {
                        ArrayBoard[killPoint.X, killPoint.Y] = null;
                    }

                    if (moveInfo.ChangePositions is { } changePositions)
                    {
                        foreach (var (startP, endP) in changePositions)
                        {
                            ArrayBoard[endP.X, endP.Y] = ArrayBoard[startP.X, startP.Y];
                            ArrayBoard[startP.X, startP.Y] = null;
                        }
                    }


                    return moveInfo;
                }
            }
            
            return new MoveInfo();
            /*else if (IsCastling(xStart, yStart, xEnd, yEnd))
            {
                if (ArrayBoard[xStart, yStart] is King king)
                {
                    var yChange = yEnd - yStart;

                    ArrayBoard[xStart, yStart] = null;
                    ArrayBoard[xEnd, yEnd] = king;
                    king.IsFirstMove = false;
                    var posChangeKing = (new Point(xStart, yStart), new Point(xEnd, yEnd));
                    if (yChange == 2 && ArrayBoard[xStart, 7] is Rook rook1)
                    {
                        ArrayBoard[xStart, 7] = null;
                        ArrayBoard[xEnd, yEnd -yChange/2] = rook1;
                        rook1.IsFirstMove = false;
                        var posChangeRook = (new Point(xStart, 7), new Point(xEnd, yEnd-yChange/2));

                        return new[] {posChangeKing,posChangeRook };
                    }
                    else if (yChange == -2 && ArrayBoard[xStart, 0] is Rook rook2)
                    {
                        ArrayBoard[xStart, 0] = null;
                        ArrayBoard[xEnd, yEnd - yChange / 2] = rook2;
                        rook2.IsFirstMove = false;
                        var posChangeRook = (new Point(xStart, 0), new Point(xEnd, yEnd - yChange / 2));
                        
                        return new[] { posChangeKing, posChangeRook };
                    }
                    else
                    {
                        throw new ApplicationException("Ошибка в IsCastling");
                    }
                }
                else
                {
                    throw new ApplicationException("Ошибка в IsCastling");
                }

            }*/

        }

        public IHaveIcon?[,] GetIcons()
        {
            return (IHaveIcon?[,]) ArrayBoard.Clone();
        }
    }

    class Board
    {
        public Piece? this[int i, int j]
        {
            get => ArrayBoard[i,j];
            protected set => ArrayBoard[i,j] = value;
        }

        protected readonly Piece?[,] ArrayBoard;

         
        public Board()
        {
            ArrayBoard = GetNewBoard();
        }

        private static Piece?[,] GetNewBoard()
        {
            Piece?[,] board = new Piece[8,8];

            #region Создание пустых ячеек
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    board[i,j] = null;
            }
            #endregion

            #region Создание белой команды

            for (int i = 0; i < 8; i++)
            {
                board[1,i] = new WhitePawn(PawnDirection.Up);
            }

            board[0,0] = new WhiteRook();
            board[0,7] = new WhiteRook();
            board[0,1] = new WhiteKnight();
            board[0,6] = new WhiteKnight();
            board[0,2] = new WhiteBishop();
            board[0,5] = new WhiteBishop();
            board[0,3] = new WhiteKing();
            board[0,4] = new WhiteQueen();

            #endregion

            #region Создание черной команды

            for (int i = 0; i < 8; i++)
            {
                board[6,i] = new BlackPawn(PawnDirection.Down);
            }

            board[7,0] = new BlackRook();
            board[7,7] = new BlackRook();
            board[7,1] = new BlackKnight();
            board[7,6] = new BlackKnight();
            board[7,2] = new BlackBishop();
            board[7,5] = new BlackBishop();
            board[7,4] = new BlackQueen();
            board[7,3] = new BlackKing();

            #endregion

            return board;
        }

        public bool CheckIsEmptySells(IEnumerable<Point>? points)
        {
            if (points is { })
            {
                foreach (var point in points)
                {
                    if (ArrayBoard[point.X, point.Y] is not null)
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }

        }
    }

}
