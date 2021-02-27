using System.Collections.Generic;

namespace Models.HexSphere
{
    public class Tile
    {
        public Point Center { get; set; }
        public List<Point> Boundary { get; set; }
    }
}
