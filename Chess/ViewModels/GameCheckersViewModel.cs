
using System.Collections.Generic;
using Chess.Models.Boards;
using Chess.Models.Boards.Base;
using Chess.Models.Players;


namespace Chess.ViewModels
{
    internal class GameCheckersViewModel:GameViewModel
    {
        public GameCheckersViewModel()
        {
            AvailablePlayers = new List<TypePlayer>
            {
                TypePlayer.SelfPlayer,
                TypePlayer.Bot1,
                TypePlayer.Bot2,
                TypePlayer.Bot3,
                TypePlayer.Bot4,
                TypePlayer.Bot5,
                TypePlayer.Bot6
            };

            SelectedFirstPlayer = TypePlayer.SelfPlayer;
            SelectedSecondPlayer = TypePlayer.Bot6;
        }


        protected override GameBoard GetNewBoard()
        {
            return new CheckersBoards(FirstPlayerTeam);
        }
    }
}
