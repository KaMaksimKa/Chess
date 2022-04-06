using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;

namespace Chess.Models.Pieces.Base
{
    public abstract class Piece:IHaveIcon
    {
        public string Icon { get; set; }
        public TeamEnum Team { get; set; }
        public int Price { get; set; }
        public bool IsFirstMove { get; set; } = true;
        public virtual List<Piece> ReplacementPieces { get; set; }= new List<Piece>();
        public double[,] PieceEval { get; set; }
        protected Piece(string icon, TeamEnum team,int price, double[,] pieceEval)
        {
            Icon = icon;
            Team = team;
            Price = team == TeamEnum.WhiteTeam ? price : -price;
            /*PieceEval = team == TeamEnum.WhiteTeam ? pieceEval : ReverseMatrix(pieceEval);*/
            PieceEval = team == TeamEnum.BlackTeam ? pieceEval : ReverseMatrix(pieceEval);
        }

        protected Piece(string icon, TeamEnum team, int price):this(icon,team,price,new double[8,8])
        {
        }
        public abstract Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board);

        private static double[,] ReverseMatrix(double[,] array)
        {
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);
            double[,] result = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[rows - i - 1, columns - j - 1] = array[i, j];
                }
            }
            return result;
        }
    }
}
