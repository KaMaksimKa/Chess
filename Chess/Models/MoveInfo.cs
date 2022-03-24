using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class MoveInfo
    {
        public IEnumerable<ChangePosition>? ChangePositions { get; set; }
        public Point? KillPoint { get; set; } 
    }
}
