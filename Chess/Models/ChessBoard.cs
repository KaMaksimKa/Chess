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
        

        private bool CheckIsEmptySells(IEnumerable<Point> points)
        {
            foreach (Point point in points)
            {
                if (ArrayBoard[point.X, point.Y] is not EmptyCell)
                    return false;
            }

            return true;
        }

        public void Move(Point pos, Point newPos)
        {
            pos = new Point(pos.Y, pos.X);
            newPos = new Point(newPos.Y, newPos.X);

            if (newPos.X > 7 || newPos.Y > 7)
            {
                throw new ApplicationException("Попытка хода за границу");
            }

            object sell = ArrayBoard[pos.X, pos.Y];
            object newSell = ArrayBoard[newPos.X, newPos.Y];
            if (sell is Piece piece)
            {
                if (piece.Team == WhoseMove)
                {
                    if (newSell is Piece newPiece)
                    {
                        if (newPiece.Team == WhoseMove)
                            throw new ApplicationException("Нельзя съесть свою фигуру");
                        else
                        {
                            try
                            {
                                if (CheckIsEmptySells(piece.GetTrajectoryForKill(pos, newPos)))
                                {
                                    ArrayBoard[newPos.X, newPos.Y] = piece;
                                    ArrayBoard[pos.X, pos.Y] = new EmptyCell();
                                    piece.IsFirstMove = false;
                                }
                                else
                                    throw new ApplicationException("Сюда нельзя походить");
                            }
                            catch (ApplicationException e)
                            {
                                throw new ApplicationException(e.Message, e);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            if (CheckIsEmptySells(piece.GetTrajectoryForMove(pos, newPos)))
                            {
                                ArrayBoard[newPos.X, newPos.Y] = piece;
                                ArrayBoard[pos.X, pos.Y] = new EmptyCell();
                                piece.IsFirstMove = false;
                            }
                            else
                                throw new ApplicationException("Сюда нельзя походить");
                        }
                        catch (ApplicationException e)
                        {
                            throw new ApplicationException(e.Message, e);
                        }

                    }
                }
                else
                    throw new ApplicationException("Это не твоя фигура");
            }
            else
                throw new ApplicationException("Пустая клетка.");
        }
    }

    class Board
    {
        public IHaveIcon this[int i, int j]
        {
            get => ArrayBoard[i,j];
            protected set => ArrayBoard[i,j] = value;
        }

        protected readonly IHaveIcon[,] ArrayBoard;

         
        public Board()
        {
            ArrayBoard = GetNewBoard();
        }

        private static IHaveIcon[,] GetNewBoard()
        {
            IHaveIcon[,] board = new IHaveIcon[8,8];

            #region Создание пустых ячеек
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    board[i,j] = new EmptyCell();
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
