using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Models.Pieces.Base;
using Chess.Models.Pieces.PiecesChess.DifferentPiece;


namespace Chess.Models.Boards
{
    internal class Chess960Board:ChessBoard
    {
        public Chess960Board():base(GetNewBoard())
        {
            
        }
        protected static Piece?[,] GetNewBoard()
        {
            var rand = new Random();

            Piece?[,] board = new Piece?[8, 8];

            #region Создание белых пешек

            for (int i = 0; i < 8; i++)
            {
                board[6, i] = new Pawn(TeamEnum.WhiteTeam,Direction.Down);
            }

            #endregion

            #region Создание черных пешек

            for (int i = 0; i < 8; i++)
            {
                board[1, i] = new Pawn(TeamEnum.BlackTeam,Direction.Up);
            }
            #endregion

            #region Создание рандомной комбинации фишера

            var set = new HashSet<int> {0,1,2,3,4,5,6,7 };

            var posKing = rand.Next(1, 7);
            board[7, posKing] = new King(TeamEnum.WhiteTeam);
            board[0, posKing] = new King(TeamEnum.BlackTeam);
            set.Remove(posKing);

            var posLeftRook = rand.Next(0, posKing);
            board[7, posLeftRook] = new Rook(TeamEnum.WhiteTeam);
            board[0, posLeftRook] = new Rook(TeamEnum.BlackTeam);
            set.Remove(posLeftRook);

            var posRightRook = rand.Next(posKing+1, 8);
            board[7, posRightRook] = new Rook(TeamEnum.WhiteTeam);
            board[0, posRightRook] = new Rook(TeamEnum.BlackTeam);
            set.Remove(posRightRook);

            var evenList = set.ToList().Where(i => i % 2 == 0).ToList();
            var posEvenBishop = evenList[rand.Next(0, evenList.Count)];
            board[7, posEvenBishop] = new Bishop(TeamEnum.WhiteTeam);
            board[0, posEvenBishop] = new Bishop(TeamEnum.BlackTeam);
            set.Remove(posEvenBishop);

            var oddList = set.ToList().Where(i => i % 2 == 1).ToList();
            var posOddBishop = oddList[rand.Next(0, oddList.Count)];
            board[7, posOddBishop] = new Bishop(TeamEnum.WhiteTeam);
            board[0, posOddBishop] = new Bishop(TeamEnum.BlackTeam);
            set.Remove(posOddBishop);

            var posKnight1 = set.ToList()[rand.Next(0, set.Count)];
            board[7, posKnight1] = new Knight(TeamEnum.WhiteTeam);
            board[0, posKnight1] = new Knight(TeamEnum.BlackTeam);
            set.Remove(posKnight1);

            var posKnight2 = set.ToList()[rand.Next(0, set.Count)];
            board[7, posKnight2] = new Knight(TeamEnum.WhiteTeam);
            board[0, posKnight2] = new Knight(TeamEnum.BlackTeam);
            set.Remove(posKnight2);

            var posQueen = set.ToList()[rand.Next(0, set.Count)];
            board[7, posQueen] = new Queen(TeamEnum.WhiteTeam);
            board[0, posQueen] = new Queen(TeamEnum.BlackTeam);
            set.Remove(posQueen);

            #endregion

            return board;

        }
    }
}
