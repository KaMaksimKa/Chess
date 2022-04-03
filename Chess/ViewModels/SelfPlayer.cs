using System;
using System.Drawing;
using System.Threading.Tasks;
using Chess.Models;
using Chess.Models.Boards.Base;
using Chess.Models.Players.Base;
using Chess.ViewModels.Base;

namespace Chess.ViewModels
{
    internal class SelfPlayer:ViewModel,IPlayer
    {
        public event Action<Point, Point>? MovedEvent;
        public event Action<Point?>? SetHintsForMoveEvent; 
        #region Свойство StartPoint
        private Point? _startPoint;
        public Point? StartPoint
        {
            get => _startPoint;
            set
            {
                Set(ref _startPoint, value);
                Task.Run(() =>
                {
                    SetHintsForMoveEvent?.Invoke(value);
                });
            }
        }

        #endregion

        #region Свойство EndPoint
        private Point? _endPoint;
        public Point? EndPoint
        {
            get => _endPoint;
            set
            {
                Set(ref _endPoint, value);
                if (StartPoint is { } startPoint && value is { } endPoint)
                {
                    Task.Run(() => MovedEvent?.Invoke(startPoint, endPoint)) ;
                    
                }
            }

                
        }
        #endregion

        public SelfPlayer(TeamEnum team)
        {
            Team = team;
        }

        
        public TeamEnum Team { get; set; }


        public void CanMovePlayer()
        {
            
        }

        public  (Point,Point)? Move(ChessBoard chessBoard)
        {
            /*if (StartPoint is { } startPoint && EndPoint is { } endPoint)
            {

                return (startPoint, endPoint);
            }*/

            return null;

        }
    }
}
