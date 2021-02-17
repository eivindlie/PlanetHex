using System.Collections.Generic;

namespace PlanetGeneration.Models
{
    public class Tile
    {
        public Point Center { get; set; }
        public List<Point> Boundary { get; set; }
    }
}
