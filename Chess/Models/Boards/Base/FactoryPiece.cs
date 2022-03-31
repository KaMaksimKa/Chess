
using Chess.Models.PiecesChess;
using Chess.Models.PiecesChess.Base;
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models.Boards.Base
{
    internal static class FactoryPiece
    {
        private static readonly BlackBishop BlackBishop  = new() {IsFirstMove = false};
        private static readonly BlackKing BlackKing  = new() { IsFirstMove = false };
        private static readonly BlackKnight BlackKnight   = new() { IsFirstMove = false };
        private static readonly BlackPawn BlackPawnUp  = new(PawnDirection.Up) { IsFirstMove = false };
        private static readonly BlackPawn BlackPawnDown  = new(PawnDirection.Down) { IsFirstMove = false };
        private static readonly BlackQueen BlackQueen  = new() { IsFirstMove = false };
        private static readonly BlackRook BlackRook  = new() { IsFirstMove = false };
        private static readonly WhiteBishop WhiteBishop= new() { IsFirstMove = false };
        private static readonly WhiteKing WhiteKing  = new() { IsFirstMove = false };
        private static readonly WhiteKnight WhiteKnight  = new() { IsFirstMove = false };
        private static readonly WhitePawn WhitePawnUp  = new(PawnDirection.Up) { IsFirstMove = false };
        private static readonly WhitePawn WhitePawnDown  = new(PawnDirection.Down) { IsFirstMove = false };
        private static readonly WhiteQueen WhiteQueen  = new() { IsFirstMove = false };
        private static readonly WhiteRook WhiteRook  = new() { IsFirstMove = false };

        public static Piece? GetMovedPiece(Piece piece)
        {
            return piece switch
            {
                PiecesChess.WhiteKing => WhiteKing,
                PiecesChess.WhiteQueen => WhiteQueen,
                PiecesChess.WhiteRook => WhiteRook,
                PiecesChess.WhiteBishop => WhiteBishop,
                PiecesChess.WhiteKnight => WhiteKnight,
                WhitePawn {Direction:PawnDirection.Up} => WhitePawnUp,
                WhitePawn {Direction:PawnDirection.Down} => WhitePawnDown,
                PiecesChess.BlackKing => BlackKing,
                PiecesChess.BlackQueen => BlackQueen,
                PiecesChess.BlackRook => BlackRook,
                PiecesChess.BlackBishop => BlackBishop,
                PiecesChess.BlackKnight => BlackKnight,
                BlackPawn { Direction: PawnDirection.Up } => BlackPawnUp,
                BlackPawn { Direction: PawnDirection.Down } => BlackPawnDown,
                _ => null
            };
        }

        
    }
}
