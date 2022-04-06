
using Chess.Models.Pieces.Base;
using Chess.Models.Pieces.PiecesChess.DifferentPiece;



namespace Chess.Models.Boards.Base
{
    internal static class FactoryPiece
    {
        private static readonly Bishop BlackBishop  = new(TeamEnum.BlackTeam) {IsFirstMove = false};
        private static readonly King BlackKing  = new(TeamEnum.BlackTeam) { IsFirstMove = false };
        private static readonly Knight BlackKnight   = new(TeamEnum.BlackTeam) { IsFirstMove = false };
        private static readonly Pawn BlackPawnUp  = new(TeamEnum.BlackTeam,Direction.Up) { IsFirstMove = false };
        private static readonly Pawn BlackPawnDown  = new(TeamEnum.BlackTeam,Direction.Down) { IsFirstMove = false };
        private static readonly Queen BlackQueen  = new(TeamEnum.BlackTeam) { IsFirstMove = false };
        private static readonly Rook BlackRook  = new(TeamEnum.BlackTeam) { IsFirstMove = false };
        private static readonly Bishop WhiteBishop= new(TeamEnum.WhiteTeam) { IsFirstMove = false };
        private static readonly King WhiteKing  = new(TeamEnum.WhiteTeam) { IsFirstMove = false };
        private static readonly Knight WhiteKnight  = new(TeamEnum.WhiteTeam) { IsFirstMove = false };
        private static readonly Pawn WhitePawnUp  = new(TeamEnum.WhiteTeam,Direction.Up) { IsFirstMove = false };
        private static readonly Pawn WhitePawnDown  = new(TeamEnum.WhiteTeam,Direction.Down) { IsFirstMove = false };
        private static readonly Queen WhiteQueen  = new(TeamEnum.WhiteTeam) { IsFirstMove = false };
        private static readonly Rook WhiteRook  = new(TeamEnum.WhiteTeam) { IsFirstMove = false };

        public static Piece? GetMovedPiece(Piece piece)
        {
            return piece switch
            {
                King{Team:TeamEnum.WhiteTeam} => WhiteKing,
                Queen { Team: TeamEnum.WhiteTeam } => WhiteQueen,
                Rook { Team: TeamEnum.WhiteTeam } => WhiteRook,
                Bishop { Team: TeamEnum.WhiteTeam } => WhiteBishop,
                Knight { Team: TeamEnum.WhiteTeam } => WhiteKnight,
                Pawn {Team:TeamEnum.WhiteTeam,Direction:Direction.Up} => WhitePawnUp,
                Pawn { Team: TeamEnum.WhiteTeam,Direction:Direction.Down} => WhitePawnDown,
                King { Team: TeamEnum.BlackTeam } => BlackKing,
                Queen { Team: TeamEnum.BlackTeam } => BlackQueen,
                Rook { Team: TeamEnum.BlackTeam } => BlackRook,
                Bishop { Team: TeamEnum.BlackTeam } => BlackBishop,
                Knight { Team: TeamEnum.BlackTeam } => BlackKnight,
                Pawn { Team: TeamEnum.BlackTeam, Direction: Direction.Up } => BlackPawnUp,
                Pawn { Team: TeamEnum.BlackTeam, Direction: Direction.Down } => BlackPawnDown,
                _ => null
            };
        }

        
    }
}
