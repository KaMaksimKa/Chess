using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;

namespace Chess.Models.Pieces.Base
{
    public abstract class Piece:IHaveIcon
    {
        public string Icon { get; init; }
        public TeamEnum Team { get; init; }
        public int Price { get; init; }
        public bool IsFirstMove { get; set; } = true;
        public List<Piece> ReplacementPieces { get; init; } = new List<Piece>();
        public double[,] PieceEval { get; init; }
        protected Piece(TypePiece typePiece, TeamEnum team,int price, double[,] pieceEval)
        {
            Icon = FactoryIcons.GetIcon(typePiece, team);
            Team = team;
            Price = team == TeamEnum.WhiteTeam ? price : -price;
            /*PieceEval = team == TeamEnum.WhiteTeam ? pieceEval : ReverseMatrix(pieceEval);*/
            PieceEval = team == TeamEnum.BlackTeam ? pieceEval : ReverseMatrix(pieceEval);
        }
        protected Piece(TypePiece typePiece, TeamEnum team, int price):this(typePiece, team,price,new double[8,8])
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
    enum Direction
    {
        Up,
        Down
    }

}
