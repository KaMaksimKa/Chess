namespace Chess.Models.Players.Base
{
    internal abstract class Player
    {
        public ChessBoard ChessBoard { get; set; }
        public TeamEnum Team { get; set; }

        protected Player(TeamEnum team,ChessBoard chessBoard)
        {
            Team = team;
            ChessBoard = chessBoard;
        }

        public abstract void Move();
    }
}
