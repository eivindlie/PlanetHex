using System.Collections.Generic;

using Models.Shared;

namespace Models.HexSphere
{
    public class Tile
    {
        public Point Center { get; set; }
        public List<Point> Boundary { get; set; }
    }
}
