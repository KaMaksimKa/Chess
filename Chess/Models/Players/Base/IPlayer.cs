using System;
using System.Drawing;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;

namespace Chess.Models.Players.Base
{
    internal  interface IPlayer
    {
        public event Action<Point, Point> MovedEvent;
        public event Action<Piece?> SetSelectedPieceEvent;
        public TeamEnum Team { get;init; }
        
        public void CanMovePlayer(Board board);

        public void SelectPiece(ChoicePiece choicePiece);

   
    }
}
