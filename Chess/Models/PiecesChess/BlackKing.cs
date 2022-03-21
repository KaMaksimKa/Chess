using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models.PiecesChess
{
    internal class BlackKing:King
    {
        public BlackKing() : base("../../Data/Img/Black/King.png", TeamEnum.BlackTeam)
        {
        }
        public override string ToString()
        {
            return "B_Kin";
        }
    }
}
