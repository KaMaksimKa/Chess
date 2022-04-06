
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Pieces.Base;

namespace Chess.Models
{
    public class MoveInfo
    {
        public ChangePosition Move { get; set; }
        public bool IsMoved { get; set; } = false;
        public IEnumerable<ChangePosition>? ChangePositions { get; set; }
        public Point? KillPoint { get; set; }

        public bool IsReplacePiece { get; set; }
        public (Point, Piece?)? ReplaceImg { get; set; }

    }
}
