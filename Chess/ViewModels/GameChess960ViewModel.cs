using System.Collections.Generic;
using Chess.Models;
using Chess.Models.Boards;
using Chess.Models.Boards.Base;
using Chess.Models.Players;

namespace Chess.ViewModels
{
    internal class GameChess960ViewModel:GameChessViewModel
    {
        protected override GameBoard GetNewBoard()
        {
            var chessBoard = new Chess960Board(FirstPlayerTeam);

            chessBoard.ChoiceReplacementPieceEvent += (pieces, whereReplace) =>
            {
                GetCurrentPlayer().SelectPiece(new ChoicePiece
                {
                    PiecesList = pieces,
                    WhereReplace = whereReplace
                });
            };

            return chessBoard;
        }
    }
}
