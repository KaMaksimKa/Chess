using System;
using System.Drawing;
using Chess.Models.Boards.Base;

namespace Chess.Models.Players.Base
{
    internal  interface IPlayer
    {
        public event Action<Point, Point> MovedEvent;
        public TeamEnum Team { get; set; }
        
        public void CanMovePlayer();

        public (Point,Point)? Move(ChessBoard chessBoard);
    }
}
