using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models.PiecesChess
{
    internal class BlackKing:King
    {
        public BlackKing() : base("Data/Black/King", TeamEnum.BlackTeam)
        {
        }
        public override string ToString()
        {
            return "B_Kin";
        }
    }
}
