using System.Collections.Generic;

namespace Chess.Models.Pieces.Base
{
    internal static class FactoryIcons
    {
        private static readonly Dictionary<(TypePiece, TeamEnum), string> DictionaryIcons =
            new Dictionary<(TypePiece, TeamEnum), string>
            {
                {(TypePiece.King, TeamEnum.WhiteTeam), "Data/Img/White/King.png" },
                {(TypePiece.Knight, TeamEnum.WhiteTeam), "Data/Img/White/Knight.png" },
                {(TypePiece.Pawn, TeamEnum.WhiteTeam), "Data/Img/White/Pawn.png" },
                {(TypePiece.Queen, TeamEnum.WhiteTeam), "Data/Img/White/Queen.png" },
                {(TypePiece.Rook, TeamEnum.WhiteTeam), "Data/Img/White/Rook.png" },
                {(TypePiece.Bishop, TeamEnum.WhiteTeam), "Data/Img/White/Bishop.png" },
                {(TypePiece.King, TeamEnum.BlackTeam), "Data/Img/Black/King.png" },
                {(TypePiece.Knight, TeamEnum.BlackTeam), "Data/Img/Black/Knight.png" },
                {(TypePiece.Pawn, TeamEnum.BlackTeam), "Data/Img/Black/Pawn.png" },
                {(TypePiece.Queen, TeamEnum.BlackTeam), "Data/Img/Black/Queen.png" },
                {(TypePiece.Rook, TeamEnum.BlackTeam), "Data/Img/Black/Rook.png" },
                {(TypePiece.Bishop, TeamEnum.BlackTeam), "Data/Img/Black/Bishop.png" },
                {(TypePiece.Disc, TeamEnum.BlackTeam), "Data/Img/Checkers/Black/Disc.png" },
                {(TypePiece.Disc, TeamEnum.WhiteTeam), "Data/Img/Checkers/White/Disc.png" }
            };

        public static string GetIcon(TypePiece typePiece, TeamEnum team)
        {
            return DictionaryIcons[(typePiece, team)];
        }

    }
}
