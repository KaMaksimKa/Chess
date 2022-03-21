using System;
using System.Collections.Generic;
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
        public abstract IEnumerable<Point> GetTrajectoryForMove(Point pos, Point newPos);
        public abstract IEnumerable<Point> GetTrajectoryForKill(Point pos, Point newPos);
    }
}
