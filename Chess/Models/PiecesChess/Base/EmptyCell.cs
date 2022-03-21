using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models.PiecesChess.Base
{
    internal class EmptyCell:IHaveIcon
    {
        public string Icon { get; set; } = String.Empty;
    }
}
