
using System.Collections.Generic;
using Chess.Models;
using Chess.Models.Boards.Base;

namespace Chess.ViewModels
{
    internal class GameChessViewModel:GameViewModel
    {
        public GameChessViewModel()
        {
            GameBoard = GetNewChessBoard(TeamEnum.WhiteTeam);
            FirstPlayer = GetNewBotChessPlayer(TeamEnum.WhiteTeam,4);
            SecondPlayer = GetNewBotChessPlayer(TeamEnum.BlackTeam, 4);

            ListChessBoards = new List<GameBoard> {(GameBoard)GameBoard.Clone()};
            CurrentBoardId = 0;
        }
    }
}
