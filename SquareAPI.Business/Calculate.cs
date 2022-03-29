namespace SquareAPI.Business
{
    public class Calculate
    {
        public SquarePoint GetSquare(int userId, IEnumerable<Point> points)
        {
            var squarePoints = new SquarePoint();
            try
            {
                squarePoints.UserId = userId;
                if (points.Count() > 3)
                {
                    // get distance of 4 combination permutation points 
                    for (int p1 = 0; p1 < points.Count() - 3; p1++)
                    {
                        var a1 = points.ElementAt(p1);
                        for (int p2 = p1 + 1; p2 < points.Count() - 2; p2++)
                        {
                            var a2 = points.ElementAt(p2);
                            for (int p3 = p2 + 1; p3 < points.Count() - 1; p3++)
                            {
                                var a3 = points.ElementAt(p3);

                                for (int p4 = p3 + 1; p4 < points.Count(); p4++)
                                {
                                    var a4 = points.ElementAt(p4);

                                    if (IsSquare(a1, a2, a3, a4))
                                    {
                                        squarePoints.Count++;
                                        var newSquare = new Square() { Points = new[] { a1, a2, a3, a4 } };
                                        squarePoints.Square.Add(newSquare);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }


            return squarePoints;
        }

        /// <summary>
        /// Check whether the provided co-ordinates form square or not.
        /// </summary>
        /// <param name="a1">First point combination of x,y plane.</param>
        /// <param name="a2">Second point combination of x,y plane.</param>
        /// <param name="a3">Third point combination of x,y plane.</param>
        /// <param name="a4">Fourth point combination of x,y plane.</param>
        /// <returns>True if co-ordinate form square else false.</returns>
        private bool IsSquare(Point a1, Point a2, Point a3, Point a4)
        {
            int distnaceToA2 = a1.DistanceToPoint(a2);
            int distanceToA3 = a1.DistanceToPoint(a3);
            int distanceToA4 = a1.DistanceToPoint(a4);

            if (distnaceToA2 == distanceToA3 && 2 * distnaceToA2 == distanceToA4)
            {
                int d = a2.DistanceToPoint(a4);
                return (d == a3.DistanceToPoint(a4) && d == distnaceToA2);
            }

            if (distanceToA3 == distanceToA4 && 2 * distanceToA3 == distnaceToA2)
            {
                int d = a2.DistanceToPoint(a3);
                return (d == a2.DistanceToPoint(a4) && d == distanceToA3);
            }

            if (distnaceToA2 == distanceToA4 && 2 * distnaceToA2 == distanceToA3)
            {
                int d = a2.DistanceToPoint(a3);
                return (d == a3.DistanceToPoint(a4) && d == distnaceToA2);
            }

            return false;
        }
    }

    public class SquarePoint
    {
        public int UserId { get; set; }
        public int Count { get; set; } = 0;

        public List<Square> Square { get; set; } = new List<Square>();
    }

    public class Square
    {
        public Point[] Points { get; set; }

        /// <summary>
        /// Overriden equal method to compare.
        /// </summary>
        /// <param name="obj">Second object to compare</param>
        /// <returns>true if the objects are equal else false</returns>
        public override bool Equals(object obj)
        {
            var square = obj as Square;

            if (square == null)
                return false;

            return square.Points[0].X == Points[0].X && square.Points[0].Y == Points[0].Y
                && square.Points[1].X == Points[1].X && square.Points[1].Y == Points[1].Y
                && square.Points[2].X == Points[2].X && square.Points[2].Y == Points[2].Y
                && square.Points[3].X == Points[3].X && square.Points[3].Y == Points[3].Y;
        }

        /// <summary>
        /// Overriden get hash code method to compare objects.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return Points[0].X ^ Points[1].X ^ Points[2].X ^ Points[3].X ^ Points[0].Y ^ Points[1].Y ^ Points[2].Y ^ Points[3].Y;
        }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Calcuate distance to another point.
        /// </summary>
        /// <param name="secondPoint">Second point to which distance needs to be measure.</param>
        /// <returns>Distance between two points</returns>
        public int DistanceToPoint(Point secondPoint)
        {
            int xAxisDistance = (X - secondPoint.X);
            int yAxisDistance = (Y - secondPoint.Y);
            return xAxisDistance * xAxisDistance + yAxisDistance * yAxisDistance;
        }
    }
}