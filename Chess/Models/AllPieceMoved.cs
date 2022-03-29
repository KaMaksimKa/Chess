using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.PiecesChess;
using Chess.Models.PiecesChess.Base;
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models
{
    internal class AllPieceMoved
    {
        private readonly BlackBishop _blackBishop  = new() {IsFirstMove = false};
        private readonly BlackKing _blackKing  = new() { IsFirstMove = false };
        private readonly BlackKnight _blackKnight   = new() { IsFirstMove = false };
        private readonly BlackPawn _blackPawnUp  = new(PawnDirection.Up) { IsFirstMove = false };
        private readonly BlackPawn _blackPawnDown  = new(PawnDirection.Down) { IsFirstMove = false };
        private readonly BlackQueen _blackQueen  = new() { IsFirstMove = false };
        private readonly BlackRook _blackRook  = new() { IsFirstMove = false };
        private readonly WhiteBishop _whiteBishop= new() { IsFirstMove = false };
        private readonly WhiteKing _whiteKing  = new() { IsFirstMove = false };
        private readonly WhiteKnight _whiteKnight  = new() { IsFirstMove = false };
        private readonly WhitePawn _whitePawnUp  = new(PawnDirection.Up) { IsFirstMove = false };
        private readonly WhitePawn _whitePawnDown  = new(PawnDirection.Down) { IsFirstMove = false };
        private readonly WhiteQueen _whiteQueen  = new() { IsFirstMove = false };
        private readonly WhiteRook _whiteRook  = new() { IsFirstMove = false };

        public Piece? GetMovedPiece(Piece piece)
        {
            return piece switch
            {
                WhiteKing => _whiteKing,
                WhiteQueen => _whiteQueen,
                WhiteRook => _whiteRook,
                WhiteBishop => _whiteBishop,
                WhiteKnight => _whiteKnight,
                WhitePawn {Direction:PawnDirection.Up} => _whitePawnUp,
                WhitePawn {Direction:PawnDirection.Down} => _whitePawnDown,
                BlackKing => _blackKing,
                BlackQueen => _blackQueen,
                BlackRook => _blackRook,
                BlackBishop => _blackBishop,
                BlackKnight => _blackKnight,
                BlackPawn { Direction: PawnDirection.Up } => _blackPawnUp,
                BlackPawn { Direction: PawnDirection.Down } => _blackPawnDown,
                _ => null
            };
        }

    }
}
