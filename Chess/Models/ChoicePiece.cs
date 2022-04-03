using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.PiecesChess.Base;

namespace Chess.Models
{
    public class ChoicePiece
    {
        public List<IHaveIcon>? IconsList { get; set; }
        public Point? WhereReplace { get; set; }
        public int? IndexReplacementPiece { get; set; } = null;

        public ChoicePiece(List<IHaveIcon> iconsList, Point whereReplace)
        {
            IconsList = iconsList;
            WhereReplace = whereReplace;
        }

        public ChoicePiece()
        {
            
        }
    }
}
