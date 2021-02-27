using System.Collections.Generic;

using PlanetGeneration.Models;

namespace Terrain
{
    public class Region
    {
        public List<Tile> Tiles { get; }

        public Region()
        {
            Tiles = new List<Tile>();
        }
    }
}
