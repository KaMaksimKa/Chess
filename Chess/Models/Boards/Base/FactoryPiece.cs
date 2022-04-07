
using System.Collections.Generic;
using Chess.Models.Pieces.Base;
using Chess.Models.Pieces.PiecesCheckers.DifferentPieces;
using Chess.Models.Pieces.PiecesChess.DifferentPiece;



namespace Chess.Models.Boards.Base
{
    internal static class FactoryPiece
    {
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
                TypePiece.KingDisc => new KingDisc(team) { IsFirstMove = isFirstMove },
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
