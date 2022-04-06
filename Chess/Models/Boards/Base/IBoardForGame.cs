using System;
using System.Drawing;


namespace Chess.Models.Boards.Base
{
    internal interface IBoardForGame
    {
        public event Action<MoveInfo>? ChessBoardMovedEvent;
        public event Action<TeamEnum?>? EndGameEvent;
        public void MakeMove(Point startPoint, Point endPoint);

    }
}
