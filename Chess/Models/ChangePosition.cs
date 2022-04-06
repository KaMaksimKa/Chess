
using System.Drawing;


namespace Chess.Models
{
    public struct ChangePosition
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public ChangePosition(Point startPoint,Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        /*public static bool operator == (ChangePosition position1, ChangePosition position2)
        {
            return position1.StartPoint==position2.StartPoint && position1.EndPoint == position2.EndPoint;
        }
        public static bool operator != (ChangePosition position1, ChangePosition position2)
        {
            return position1.StartPoint != position2.StartPoint || position1.EndPoint != position2.EndPoint;
        }*/
        public void Deconstruct(out Point startPoint, out Point endPoint)
        {
            startPoint = StartPoint;
            endPoint = EndPoint;
        }
    }
}
