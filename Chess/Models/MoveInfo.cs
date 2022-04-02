
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.PiecesChess.Base;

namespace Chess.Models
{
    public class MoveInfo
    {
        public ChangePosition Move { get; set; }
        public IEnumerable<ChangePosition>? ChangePositions { get; set; }
        public Point? KillPoint { get; set; }
        public (Point, IHaveIcon)? ReplaceImg { get; set; }
    }
}
