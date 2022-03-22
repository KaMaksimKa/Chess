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


        public bool IsMove(Point pos, Point newPos)
        {
            if (newPos.X > 7 || newPos.Y > 7)
            {
                return false;
            }

            object sell = ArrayBoard[pos.X, pos.Y];
            object newSell = ArrayBoard[newPos.X, newPos.Y];

            if (sell is Piece piece)
            {
                if (piece.Team == WhoseMove)
                {
                    if (newSell is Piece)
                    {
                        return false;
                    }
                    else
                    {
                        try
                        {
                            return CheckIsEmptySells(piece.GetTrajectoryForMove(pos, newPos));
                        }
                        catch (ApplicationException)
                        {
                            return false;
                        }
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool IsKill(Point pos, Point newPos)
        {
            if (newPos.X > 7 || newPos.Y > 7)
            {
                return false;
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
                            return false;
                        else
                        {
                            try
                            {
                                return CheckIsEmptySells(piece.GetTrajectoryForKill(pos, newPos));
                            }
                            catch (ApplicationException)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }


        public void Move(Point pos, Point newPos)
        {

            if (IsKill(pos, newPos) || IsMove(pos, newPos))
            {
                ((Piece) ArrayBoard[pos.X, pos.Y]).IsFirstMove = false;
                ArrayBoard[newPos.X, newPos.Y] = ArrayBoard[pos.X, pos.Y];
                ArrayBoard[pos.X, pos.Y] = new EmptyCell();
            }
            else
            {
                throw new ApplicationException("Сюда ходить нельзя");
            }
            
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
