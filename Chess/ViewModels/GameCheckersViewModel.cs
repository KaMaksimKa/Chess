using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models;
using Chess.Models.Boards.Base;

namespace Chess.ViewModels
{
    internal class GameCheckersViewModel:GameViewModel
    {
        public GameCheckersViewModel()
        {
            GameBoard = GetNewCheckersBoard(TeamEnum.WhiteTeam);
            FirstPlayer = GetNewBotPlayer(TeamEnum.WhiteTeam,6);
            SecondPlayer = GetNewBotPlayer(TeamEnum.BlackTeam, 6);

            ListChessBoards = new List<GameBoard> {(GameBoard)GameBoard.Clone()};
            CurrentBoardId = 0;
        }
    }
}
