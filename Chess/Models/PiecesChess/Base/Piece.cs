using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;

namespace Chess.Models.PiecesChess.Base
{
    internal abstract class Piece:IHaveIcon
    {
        public string Icon { get; set; }
        public TeamEnum Team { get; set; }
        public int Price { get; set; }
        public bool IsFirstMove { get; set; } = true;

        protected Piece(string icon, TeamEnum team,int price)
        {
            Icon = icon;
            Team = team;
            Price = price;

        }
        public abstract Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board);
    }
}
