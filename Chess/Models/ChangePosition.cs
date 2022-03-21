
using System.Drawing;


namespace Chess.Models
{
    public class ChangePosition
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public ChangePosition()
        {
            
        }

        public ChangePosition(Point startPoint,Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public void Deconstruct(out Point startPoint, out Point endPoint)
        {
            startPoint = StartPoint;
            endPoint = EndPoint;
        }
    }
}
