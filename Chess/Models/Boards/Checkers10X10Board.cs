using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;

namespace Chess.Models.Boards
{
    internal class Checkers10X10Board:CheckersBoard
    {
        public Checkers10X10Board(TeamEnum team) : base(GetNewBoard(team))
        {
        }

        public static Piece?[,] GetNewBoard(TeamEnum teamDown)
        {
            Piece?[,] board = new Piece?[10, 10];

            TeamEnum teamUp = teamDown == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;

            var factoryDown = FactoryPiece.GetFactory(teamDown, Direction.Down, true);
            var factoryUp = FactoryPiece.GetFactory(teamUp, Direction.Up, true);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j += 2)
                {
                    board[9 - i, j + i % 2] = factoryDown.GetPiece(TypePiece.Disc);
                }
            }


            for (int i = 0; i < 3; i++)
            {
                for (int j = 1; j < 10; j += 2)
                {
                    board[0 + i, j - i % 2] = factoryUp.GetPiece(TypePiece.Disc);
                }
            }

            return board;
        }
    }
}
