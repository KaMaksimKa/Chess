using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
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
