namespace Models.HexSphere
{
    public class Face
    {
        public Point[] Points { get; set; }

        private Point _centroid;
        
        public Point Centroid
        {
            get
            {
                if (_centroid != null) return _centroid;
                
                var x = (Points[0].X + Points[1].X + Points[2].X) / 3;
                var y = (Points[0].Y + Points[1].Y + Points[2].Y) / 3;
                var z = (Points[0].Z + Points[1].Z + Points[2].Z) / 3;

                _centroid = new Point(x, y, z);

                return _centroid;
            }
        }

        public Face(Point p1, Point p2, Point p3, bool registerFace = false)
        {
            Points = new[] { p1, p2, p3 };

            if (registerFace)
            {
                foreach (var p in Points)
                {
                    p.RegisterFace(this);
                }
            }
        }
        
        public bool IsAdjacentTo(Face face)
        {
            var count = 0;
            foreach (var p1 in Points)
            {
                foreach (var p2 in face.Points)
                {
                    if (p1 == p2)
                    {
                        count++;
                    }
                }
            }
            return count == 2;
        }

        public override string ToString()
        {
            return $"Face({Points[0]}, {Points[1]}, {Points[2]})";
        }
    }
}
