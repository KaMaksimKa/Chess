using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class HintsChess
    {
        public bool[,]? IsHintsForMove { get; set; } 
        public bool[,]? IsHintsForKill { get; set; }

    }
}
