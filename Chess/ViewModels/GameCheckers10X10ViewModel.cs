using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.Boards;
using Chess.Models.Boards.Base;
using Chess.Models.Players;

namespace Chess.ViewModels
{
    internal class GameCheckers10X10ViewModel:GameCheckersViewModel
    {
        public GameCheckers10X10ViewModel()
        {
            AvailablePlayers = new List<TypePlayer>
            {
                TypePlayer.SelfPlayer,
                TypePlayer.Bot1,
                TypePlayer.Bot2,
                TypePlayer.Bot3,
                TypePlayer.Bot4,
                TypePlayer.Bot5
            };

            SelectedFirstPlayer = TypePlayer.SelfPlayer;
            SelectedSecondPlayer = TypePlayer.Bot5;
        }
        protected override GameBoard GetNewBoard()
        {
            return new Checkers10X10Board(FirstPlayerTeam);
        }
    }
}
