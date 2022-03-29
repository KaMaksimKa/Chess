﻿using System.Collections.Generic;
using System.Drawing;

namespace Chess.Models.PiecesChess.Base
{
    internal abstract class Piece:IHaveIcon
    {
        public string Icon { get; set; }
        public TeamEnum Team { get; set; }
        public bool IsFirstMove { get; set; } = true;

        protected Piece(string icon, TeamEnum team)
        {
            Icon = icon;
            Team = team;

        }
        public abstract Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board);
    }
}
