using System.Collections.Generic;

namespace PlanetGeneration.Models
{
    public class Polyhedron
    {
        public List<Point> Corners { get; set; }
        public List<Face> Faces { get; set; }
    }
}
