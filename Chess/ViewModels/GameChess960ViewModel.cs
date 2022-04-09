using System.Collections.Generic;
using Chess.Models;
using Chess.Models.Boards;
using Chess.Models.Boards.Base;
using Chess.Models.Players;

namespace Chess.ViewModels
{
    internal class GameChess960ViewModel:GameViewModel
    {
        public GameChess960ViewModel()
        {
            AvailablePlayers = new List<TypePlayer>
            {
                TypePlayer.SelfPlayer,
                TypePlayer.ChessBot1,
                TypePlayer.ChessBot2,
                TypePlayer.ChessBot3,
                TypePlayer.ChessBot4
            };

            SelectedFirstPlayer = TypePlayer.SelfPlayer;
            SelectedSecondPlayer = TypePlayer.ChessBot4;
        }
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
