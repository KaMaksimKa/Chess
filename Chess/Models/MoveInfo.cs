
using System.Collections.Generic;
using System.Drawing;

namespace Chess.Models
{
    public class MoveInfo
    {
        public IEnumerable<ChangePosition>? ChangePositions { get; set; }
        public Point? KillPoint { get; set; } 
    }
}
