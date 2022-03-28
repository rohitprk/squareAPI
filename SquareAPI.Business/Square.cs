namespace SquareAPI.Business
{
    public class Square
    {
        public void GetSquare(Point a1, Point a2, Point a3, Point a4)
        {
            int distnaceToA2 = a1.DistanceToPoint(a2);
            int distanceToA3 = a1.DistanceToPoint(a3);
            int distanceToA4 = a1.DistanceToPoint(a4);

            if (distnaceToA2 == distanceToA4 && 2*distnaceToA2 == distanceToA4 && 2*a2.DistanceToPoint(a4) == a2.DistanceToPoint(a3))
            {
                //return true;
            }

            if (distanceToA3 == distanceToA4 && 2*distnaceToA2== distanceToA3 && 2*a2.DistanceToPoint(a3) == a2.DistanceToPoint(a4))
            {
                //return true;
            }

           // return false;
        }
    }


    public class Point
    {
        public int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int DistanceToPoint(Point secondPoint)
        {
            int xAxisDistance = (x -secondPoint.x);
            int yAxisDistance = (y - secondPoint.y);
            return xAxisDistance * xAxisDistance + yAxisDistance *yAxisDistance; 
        }
    }
}