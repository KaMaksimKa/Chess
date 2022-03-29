using System.Drawing;
using System.Threading.Tasks;
using Chess.Models.Boards.Base;
using Chess.Models.Players.Base;

namespace Chess.Models.Players
{
    internal class SelfPlayer:Player
    {
        public Point? StartPoint { get; set; }
        public Point? EndPoint { get; set; }
        public SelfPlayer(TeamEnum team, ChessBoard chessBoard) : base(team, chessBoard)
        {
        }

        public override async void Move()
        {
            await Task.Run(() =>
            {
                if (StartPoint is { } startPoint && EndPoint is { } endPoint)
                {
                    ChessBoard.Move(startPoint, endPoint);
                }
            });
            
           
        }
    }
}
