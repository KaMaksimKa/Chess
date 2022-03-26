using System.Collections.Generic;
using System.Drawing;

namespace Chess.Models
{
    public class HintsChess
    {
        public List<Point> IsHintsForMove { get; set; } = new List<Point>();
        public List<Point> IsHintsForKill { get; set; } = new List<Point>();

    }
}
