using BasicLibraryWinForm.PointFolder;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace BasicLibraryWinForm
{

    /// <summary>
    /// Алгоритм Вельцля
    /// </summary>
    public sealed class SmallestEnclosingCircle
    {


        /// <summary>
        ///          Returns the smallest circle that encloses all the given points. Runs in expected O(n) time, randomized.
        /// Note: If 0 points are given, a circle of radius -1 is returned.If 1 point is given, a circle of radius 0 is returned.
        ///Initially: No boundary points known
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Circle MakeCircle(IList<Point> points)
        {
            // Clone list to preserve the caller's data, do Durstenfeld shuffle
            List<Point> shuffled = new(points);
            Random rand = new();
            for (int i = shuffled.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                var temp = shuffled[i];
                shuffled[i] = shuffled[j];
                shuffled[j] = temp;
            }

            // Progressively add points to circle or recompute circle
            Circle c = Circle.INVALID;
            for (int i = 0; i < shuffled.Count; i++)
            {
                Point p = shuffled[i];
                if (c.Radius < 0 || !c.Contains(p))
                    c = MakeCircleOnePoint(shuffled.GetRange(0, i + 1), p);
            }
            return c;
        }


        // One boundary point known
        private static Circle MakeCircleOnePoint(List<Point> points, Point p)
        {
            Circle c = new(p, 0);
            for (int i = 0; i < points.Count; i++)
            {
                Point q = points[i];
                if (!c.Contains(q))
                {
                    if (c.Radius == 0)
                        c = MakeDiameter(p, q);
                    else
                        c = MakeCircleTwoPoints(points.GetRange(0, i + 1), p, q);
                }
            }
            return c;
        }


        // Two boundary points known
        private static Circle MakeCircleTwoPoints(List<Point> points, Point p, Point q)
        {
            Circle circ = MakeDiameter(p, q);
            Circle left = Circle.INVALID;
            Circle right = Circle.INVALID;

            // For each point not in the two-point circle
            Point pq = q - p;
            foreach (Point r in points)
            {
                if (circ.Contains(r))
                    continue;

                // Form a circumcircle and classify it on left or right side
                double cross = Cross(pq, r - p);
                Circle circle = MakeCircumcircle(p, q, r);
                if (circle.Radius < 0)
                    continue;
                else if (cross > 0 && (left.Radius < 0 || Cross(pq, circle.Center - p) > Cross(pq, left.Center - p)))
                    left = circle;
                else if (cross < 0 && (right.Radius < 0 || Cross(pq, circle.Center - p) < Cross(pq, right.Center - p)))
                    right = circle;
            }

            // Select which circle to return
            if (left.Radius < 0 && right.Radius < 0)
                return circ;
            else if (left.Radius < 0)
                return right;
            else if (right.Radius < 0)
                return left;
            else
                return left.Radius <= right.Radius ? left : right;
        }


        public static Circle MakeDiameter(Point a, Point b)
        {
            Point c = (a + b) / 2;
            return new Circle(c, Math.Max(c.GetDistance(a), c.GetDistance(b)));
        }


        public static Circle MakeCircumcircle(Point a, Point b, Point c)
        {
            // Mathematical algorithm from Wikipedia: Circumscribed circle
            double ox = (Math.Min(Math.Min(a.X, b.X), c.X) + Math.Max(Math.Max(a.X, b.X), c.X)) / 2;
            double oy = (Math.Min(Math.Min(a.Y, b.Y), c.Y) + Math.Max(Math.Max(a.Y, b.Y), c.Y)) / 2;
            double ax = a.X - ox, ay = a.Y - oy;
            double bx = b.X - ox, by = b.Y - oy;
            double cx = c.X - ox, cy = c.Y - oy;
            double d = (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by)) * 2;
            if (d == 0)
                return Circle.INVALID;
            double x = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
            double y = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;
            Point p = new(ox + x, oy + y);
            double r = Math.Max(Math.Max(p.GetDistance(a), p.GetDistance(b)), p.GetDistance(c));
            return new Circle(p, r);
        }

        private static double Cross(Point p1, Point p2)
        {
            return p1.X * p2.Y - p1.Y * p2.X;
        }

    }
}
