using System.Collections.Generic;

namespace Models.HexSphere
{
    public class Polyhedron
    {
        public List<Point> Corners { get; set; }
        public List<Face> Faces { get; set; }
    }
}
