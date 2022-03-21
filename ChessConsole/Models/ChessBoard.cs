using System;
using System.Collections.Generic;
using System.Drawing;
using ChessConsole.Models;
using ChessConsole.Models.PiecesChess;
using ChessConsole.Models.PiecesChess.Base;
using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models
{
    internal class ChessBoard
    {
        public TeamEnum WhoseMove { get; set; } = TeamEnum.WhiteTeam;

        public Board Board { get; set; }

        public ChessBoard()
        {
            Board = new Board();
        }



        private bool CheckIsEmptySells(IEnumerable<Point> points)
        {
            foreach (Point point in points)
            {
                if (Board[point.X, point.Y] is not EmptyCell)
                    return false;
            }

            return true;
        }

        public void Move(Point pos, Point newPos)
        {
            pos = new Point(pos.Y, pos.X);
            newPos = new Point(newPos.Y, newPos.X);

            object sell = Board[pos.X, pos.Y];
            object newSell = Board[newPos.X, newPos.Y];
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
                                    Board[newPos.X, newPos.Y] = piece;
                                    Board[pos.X, pos.Y] = new EmptyCell();
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
                                Board[newPos.X, newPos.Y] = piece;
                                Board[pos.X, pos.Y] = new EmptyCell();
                                piece.IsFirstMove = false;
                            }
                            else
                                throw new ApplicationException("Сюда нельзя");
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
        public object this[int i, int j]
        {
            get => ListBoard[i * 8 + j];
            set => ListBoard[i * 8 + j] = value;
        }
        public object[] ListBoard { get; set; }


        public Board()
        {
            ListBoard = GetNewBoard();
        }

        private static object[] GetNewBoard()
        {
            object[] board = new object[64];

            #region Создание пустых ячеек
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    board[i * 8 + j] = new EmptyCell();
            }
            #endregion

            #region Создание белой команды

            for (int i = 0; i < 8; i++)
            {
                board[1 * 8 + i] = new WhitePawn(PawnDirection.Up);
            }

            board[0] = new WhiteRook();
            board[7] = new WhiteRook();
            board[1] = new WhiteKnight();
            board[6] = new WhiteKnight();
            board[2] = new WhiteBishop();
            board[5] = new WhiteBishop();
            board[3] = new WhiteKing();
            board[4] = new WhiteQueen();

            #endregion

            #region Создание черной команды

            for (int i = 0; i < 8; i++)
            {
                board[6 * 8 + i] = new BlackPawn(PawnDirection.Up);
            }

            board[7 * 8 + 0] = new BlackRook();
            board[7 * 8 + 7] = new BlackRook();
            board[7 * 8 + 1] = new BlackKnight();
            board[7 * 8 + 6] = new BlackKnight();
            board[7 * 8 + 2] = new BlackBishop();
            board[7 * 8 + 5] = new BlackBishop();
            board[7 * 8 + 4] = new BlackQueen();
            board[7 * 8 + 3] = new BlackKing();

            #endregion

            return board;
        }
    }

    internal class EmptyCell
    {
        public override string ToString()
        {
            return "Empty";
        }
    }

}

/*using System.Drawing;
using ChessConsole.Models.PiecesChess;
using ChessConsole.Models.PiecesChess.Base;
using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models
{
    internal class ChessBoard
    {
        public TeamEnum WhoseMove { get; set; } = TeamEnum.WhiteTeam;

        private object[,] _board;

        public ChessBoard()
        {
            _board = GetNewBoard();
        }

        private static object[,] GetNewBoard()
        {
            object[,] board = new object[8, 8];

            #region Создание пустых ячеек
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    board[i, j] = new EmptySell();
            }
            #endregion

            #region Создание белой команды

            for (int i = 0; i < 8; i++)
            {
                board[1, i] = new WhitePawn(PawnDirection.Up);
            }

            board[0, 0] = new WhiteRook();
            board[0, 7] = new WhiteRook();
            board[0, 1] = new WhiteKnight();
            board[0, 6] = new WhiteKnight();
            board[0, 2] = new WhiteBishop();
            board[0, 5] = new WhiteBishop();
            board[0, 3] = new WhiteKing();
            board[0, 4] = new WhiteQueen();

            #endregion

            #region Создание черной команды

            for (int i = 0; i < 8; i++)
            {
                board[6, i] = new WhitePawn(PawnDirection.Up);
            }

            board[7, 0] = new WhiteRook();
            board[7, 7] = new WhiteRook();
            board[7, 1] = new WhiteKnight();
            board[7, 6] = new WhiteKnight();
            board[7, 2] = new WhiteBishop();
            board[7, 5] = new WhiteBishop();
            board[7, 4] = new WhiteQueen();
            board[7, 3] = new WhiteKing();

            #endregion

            return board;
        }

        private bool CheckIsEmptySells(IEnumerable<Point> points)
        {
            foreach (Point point in points)
            {
                if (_board[point.X, point.Y] is not EmptySell)
                    return false;
            }

            return true;
        }

        public void Move(Point pos, Point newPos)
        {
            pos = new Point(pos.Y, pos.X);
            newPos = new Point(newPos.Y, newPos.X);

            object sell = _board[pos.X, pos.Y];
            object newSell = _board[newPos.X, newPos.Y];
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
                                    _board[newPos.X, newPos.Y] = piece;
                                    _board[pos.X, pos.Y] = new EmptySell();
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
                                _board[newPos.X, newPos.Y] = piece;
                                _board[pos.X, pos.Y] = new EmptySell();
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


    internal class EmptySell{}

}
*/