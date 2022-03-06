namespace PlanetHex.PlanetGeneration.Models.HexSphere
{
    public class Face
    {
        public HexSpherePoint[] Points { get; set; }

        private HexSpherePoint? _centroid;

        public HexSpherePoint Centroid
        {
            get
            {
                if (_centroid != null) return _centroid;

                var x = (Points[0].X + Points[1].X + Points[2].X) / 3;
                var y = (Points[0].Y + Points[1].Y + Points[2].Y) / 3;
                var z = (Points[0].Z + Points[1].Z + Points[2].Z) / 3;

                _centroid = new HexSpherePoint(x, y, z);

                return _centroid;
            }
        }

        public Face(HexSpherePoint p1, HexSpherePoint p2, HexSpherePoint p3, bool registerFace = false)
        {
            Points = new[] { p1, p2, p3 };

            if (!registerFace) return;
            foreach (var p in Points)
            {
                p.RegisterFace(this);
            }
        }

        public override string ToString()
        {
            return $"Face({Points[0]}, {Points[1]}, {Points[2]})";
        }
    }
}
