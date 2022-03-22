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
            if (points == null)
            {
                return false;
            }
            foreach (var (x,y) in points)
            {
                if (ArrayBoard[x, y] is not EmptyCell)
                    return false;
            }

            return true;
        }


        public bool IsMove(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {

            object sell = ArrayBoard[xStart, yStart];
            object newSell = ArrayBoard[xEnd, yEnd];

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
                        return CheckIsEmptySells(piece.GetTrajectoryForMove(xStart,yStart,xEnd,yEnd ));
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool IsKill(byte xStart, byte yStart, byte xEnd, byte yEnd)
        {

            object sell = ArrayBoard[xStart, yStart];
            object newSell = ArrayBoard[xEnd, yEnd];

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
                                return CheckIsEmptySells(piece.GetTrajectoryForKill(xStart,yStart, xEnd,yEnd));
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


        public void Move(byte xStart,byte yStart,byte xEnd,byte yEnd)
        {
            if (xStart>7 || yStart>7 || xEnd>7 || yEnd>7)
            {
                throw new ApplicationException("Сюда ходить нельзя");
            }

            if (IsKill(xStart,yStart,xEnd,yEnd) || IsMove(xStart, yStart, xEnd, yEnd))
            {
                ((Piece) ArrayBoard[xStart, yStart]).IsFirstMove = false;
                ArrayBoard[xEnd, yEnd] = ArrayBoard[xStart, yStart];
                ArrayBoard[xStart, yStart] = new EmptyCell();
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
