using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;


namespace Chess.Models.Boards
{
    internal class Chess960Board:ChessBoard
    {
        public Chess960Board(TeamEnum teamDown) :base(GetNewBoard(teamDown))
        {
            
        }
        protected static Piece?[,] GetNewBoard(TeamEnum teamDown)
        {
            TeamEnum teamUp = teamDown == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;

            var factoryDown = FactoryPiece.GetFactory(teamDown, Direction.Down, true);
            var factoryUp = FactoryPiece.GetFactory(teamUp, Direction.Up, true);

            var rand = new Random();

            Piece?[,] board = new Piece?[8, 8];

           

            for (int i = 0; i < 8; i++)
            {
                board[6, i] = factoryDown.GetPiece(TypePiece.Pawn);
            }


            for (int i = 0; i < 8; i++)
            {
                board[1, i] = factoryUp.GetPiece(TypePiece.Pawn);
            }
        

            #region Создание рандомной комбинации фишера

            var set = new HashSet<int> {0,1,2,3,4,5,6,7 };

            var posKing = rand.Next(1, 7);
            board[7, posKing] = factoryDown.GetPiece(TypePiece.King);
            board[0, posKing] = factoryUp.GetPiece(TypePiece.King);
            set.Remove(posKing);

            var posLeftRook = rand.Next(0, posKing);
            board[7, posLeftRook] = factoryDown.GetPiece(TypePiece.Rook);
            board[0, posLeftRook] = factoryUp.GetPiece(TypePiece.Rook);
            set.Remove(posLeftRook);

            var posRightRook = rand.Next(posKing+1, 8);
            board[7, posRightRook] = factoryDown.GetPiece(TypePiece.Rook);
            board[0, posRightRook] = factoryUp.GetPiece(TypePiece.Rook);
            set.Remove(posRightRook);

            var evenList = set.ToList().Where(i => i % 2 == 0).ToList();
            var posEvenBishop = evenList[rand.Next(0, evenList.Count)];
            board[7, posEvenBishop] = factoryDown.GetPiece(TypePiece.Bishop);
            board[0, posEvenBishop] = factoryUp.GetPiece(TypePiece.Bishop);
            set.Remove(posEvenBishop);

            var oddList = set.ToList().Where(i => i % 2 == 1).ToList();
            var posOddBishop = oddList[rand.Next(0, oddList.Count)];
            board[7, posOddBishop] = factoryDown.GetPiece(TypePiece.Bishop);
            board[0, posOddBishop] = factoryUp.GetPiece(TypePiece.Bishop);
            set.Remove(posOddBishop);

            var posKnight1 = set.ToList()[rand.Next(0, set.Count)];
            board[7, posKnight1] = factoryDown.GetPiece(TypePiece.Knight);
            board[0, posKnight1] = factoryUp.GetPiece(TypePiece.Knight);
            set.Remove(posKnight1);

            var posKnight2 = set.ToList()[rand.Next(0, set.Count)];
            board[7, posKnight2] = factoryDown.GetPiece(TypePiece.Knight);
            board[0, posKnight2] = factoryUp.GetPiece(TypePiece.Knight);
            set.Remove(posKnight2);

            var posQueen = set.ToList()[rand.Next(0, set.Count)];
            board[7, posQueen] = factoryDown.GetPiece(TypePiece.Queen);
            board[0, posQueen] = factoryUp.GetPiece(TypePiece.Queen);
            set.Remove(posQueen);

            #endregion

            return board;

        }
    }
}
