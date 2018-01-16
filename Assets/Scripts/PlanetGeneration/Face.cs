using System.Linq;

namespace PlanetGeneration
{
    public class Face
    {
        public Point[] points;
        private Point Centroid;

        public Face(Point p1, Point p2, Point p3, bool registerFace = true)
        {
            this.points = new Point[] { p1, p2, p3 };
            if(registerFace)
            {
                p1.RegisterFace(this);
                p2.RegisterFace(this);
                p3.RegisterFace(this);
            }
        }

        public Point[] GetOtherPoints(Point p)
        {
            return this.points.Where(p2 => p2 != p).ToArray();
        }

        public Point FindThirdPoint(Point p1, Point p2)
        {
            return this.points.Where(p3 => p3 != p1 && p3 != p2).First();
        }

        public bool IsAdjacentTo(Face face)
        {
            var count = 0;
            foreach (var p1 in points)
            {
                foreach (var p2 in face.points)
                {
                    if (p1 == p2)
                    {
                        count++;
                    }
                }
            }
            return count == 2;
        }

        public Point GetCentroid(bool clear = false)
        {
            if (Centroid != null && !clear)
                return Centroid;
            
            var x = (this.points[0].x + this.points[1].x + this.points[2].x) / 3;
            var y = (this.points[0].y + this.points[1].y + this.points[2].y) / 3;
            var z = (this.points[0].z + this.points[1].z + this.points[2].z) / 3;

            Centroid = new Point(x, y, z);

            return Centroid;
        }
    }
}