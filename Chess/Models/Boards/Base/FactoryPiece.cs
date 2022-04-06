
using System.Collections.Generic;
using Chess.Models.Pieces.Base;
using Chess.Models.Pieces.PiecesCheckers.DifferentPieces;
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


        private static readonly Dictionary<(TypePiece, TeamEnum, Direction, bool), Piece?> DictionaryPieces;
           

        static FactoryPiece()
        {
            DictionaryPieces = new Dictionary<(TypePiece, TeamEnum, Direction, bool), Piece?>();
        }
        public static Piece? GetPiece(TypePiece typePiece, TeamEnum team, Direction direction, bool isFirstMove)
        {
            if (!DictionaryPieces.ContainsKey((typePiece, team, direction, isFirstMove)))
            {
                DictionaryPieces.Add((typePiece, team, direction, isFirstMove), GetNewPiece(typePiece, team, direction, isFirstMove));
            }
            
            return DictionaryPieces[(typePiece, team, direction, isFirstMove)];
        }
        private static Piece? GetNewPiece(TypePiece typePiece, TeamEnum team, Direction direction, bool isFirstMove)
        {
            return typePiece switch
            {
                TypePiece.Bishop=>new Bishop(team){IsFirstMove = isFirstMove},
                TypePiece.King => new King(team) { IsFirstMove = isFirstMove },
                TypePiece.Knight => new Knight(team) { IsFirstMove = isFirstMove },
                TypePiece.Queen => new Queen(team) { IsFirstMove = isFirstMove },
                TypePiece.Rook => new Rook(team) { IsFirstMove = isFirstMove },
                TypePiece.Pawn => new Pawn(team,direction) { IsFirstMove = isFirstMove },
                TypePiece.Disc=> new Disc(team, direction) { IsFirstMove = isFirstMove },
                _ =>null
            };
        }
        public static SpecialFactoryPiece GetFactory(TeamEnum team, Direction direction, bool isFirstMove)
        {
            return new SpecialFactoryPiece(team, direction, isFirstMove);
        }
        public class SpecialFactoryPiece
        {
            public  TeamEnum Team { get; init; }
            public Direction Direction { get; init; }
            public bool IsFirstMove { get; init; }

            public SpecialFactoryPiece(TeamEnum team,Direction direction,bool isFirstMove)
            {  
                Team = team;
                Direction = direction;
                IsFirstMove = isFirstMove;
            }

            public Piece? GetPiece(TypePiece typePiece)
            {
                return FactoryPiece.GetPiece(typePiece, Team, Direction, IsFirstMove);
            }
        }

    }

    
}
