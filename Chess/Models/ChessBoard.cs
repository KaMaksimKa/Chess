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
        

        private bool CheckIsEmptySells(IEnumerable<(byte,byte)>? points)
        {
            if (points is {})
            {
                foreach (var (x, y) in points)
                {
                    if (ArrayBoard[x, y] is not null)
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


        public bool IsMove(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {
            
            object? sell = ArrayBoard[xStart, yStart];
            object? newSell = ArrayBoard[xEnd, yEnd];

            if (sell is Piece piece && piece.Team == WhoseMove && newSell is not Piece)
            {
                return CheckIsEmptySells(piece.GetTrajectoryForMove(xStart, yStart, xEnd, yEnd));
            }
            return false;
        }

        public bool IsKill(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {

            object? sell = ArrayBoard[xStart, yStart];
            object? newSell = ArrayBoard[xEnd, yEnd];

            if (sell is Piece piece && piece.Team == WhoseMove &&
                (newSell is Piece newPiece) && newPiece.Team != WhoseMove)
            {
                return CheckIsEmptySells(piece.GetTrajectoryForKill(xStart, yStart, xEnd, yEnd));
            }
            return false;
        }

        public bool IsCastling(byte xStart, byte yStart, byte xEnd, byte yEnd)
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
        }

        public IEnumerable<(Point,Point)> Move(byte xStart,byte yStart,byte xEnd,byte yEnd)
        {
            if (xStart>7 || yStart>7 || xEnd>7 || yEnd>7)
            {
                throw new ApplicationException("Сюда ходить нельзя");
            }

            if (IsKill(xStart,yStart,xEnd,yEnd) || IsMove(xStart, yStart, xEnd, yEnd))
            {
                if (ArrayBoard[xStart, yStart] is { } piece)
                    piece.IsFirstMove = false;
                ArrayBoard[xEnd, yEnd] = ArrayBoard[xStart, yStart];
                ArrayBoard[xStart, yStart] = null;
                return new List<(Point,Point)>
                {
                    (new Point(xStart,yStart),new Point(xEnd,yEnd))
                };
            }
            else if (IsCastling(xStart, yStart, xEnd, yEnd))
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

            }
            else
            {
                throw new ApplicationException("Сюда ходить нельзя");
            }
            
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
    }

}
