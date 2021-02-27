using System.Collections.Generic;
using System.Linq;

using Models.Shared;

namespace Models.HexSphere
{
    public class TileRegion
    {
        public List<Tile> Tiles { get; set; }

        public Point Center
        {
            get
            {
                var sum = Tiles.Select(t => t.Center.AsVector()).Aggregate((v1, v2) => v1 + v2);
                var centerVector = sum / Tiles.Count;
                return new Point
                {
                    X = centerVector.x,
                    Y = centerVector.y,
                    Z = centerVector.z
                };
            }
        }
    }
}
