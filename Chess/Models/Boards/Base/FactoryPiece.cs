
using Chess.Models.Pieces.Base;
using Chess.Models.Pieces.PiecesChess;
using Chess.Models.Pieces.PiecesChess.DifferentPiece;



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
                Pieces.PiecesChess.WhiteKing => WhiteKing,
                Pieces.PiecesChess.WhiteQueen => WhiteQueen,
                Pieces.PiecesChess.WhiteRook => WhiteRook,
                Pieces.PiecesChess.WhiteBishop => WhiteBishop,
                Pieces.PiecesChess.WhiteKnight => WhiteKnight,
                WhitePawn {Direction:PawnDirection.Up} => WhitePawnUp,
                WhitePawn {Direction:PawnDirection.Down} => WhitePawnDown,
                Pieces.PiecesChess.BlackKing => BlackKing,
                Pieces.PiecesChess.BlackQueen => BlackQueen,
                Pieces.PiecesChess.BlackRook => BlackRook,
                Pieces.PiecesChess.BlackBishop => BlackBishop,
                Pieces.PiecesChess.BlackKnight => BlackKnight,
                BlackPawn { Direction: PawnDirection.Up } => BlackPawnUp,
                BlackPawn { Direction: PawnDirection.Down } => BlackPawnDown,
                _ => null
            };
        }

        
    }
}
