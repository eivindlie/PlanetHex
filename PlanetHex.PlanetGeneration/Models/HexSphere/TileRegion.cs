using PlanetHex.PlanetGeneration.Models.Shared;

namespace PlanetHex.PlanetGeneration.Models.HexSphere
{
    public class TileRegion
    {
        public List<Tile> Tiles { get; set; } = new();

        public Point Center
        {
            get
            {
                var sum = Tiles.Select(t => t.Center.AsVector()).Aggregate((v1, v2) => v1 + v2);
                var centerVector = sum / Tiles.Count;
                return new Point
                {
                    X = centerVector.X,
                    Y = centerVector.Y,
                    Z = centerVector.Z
                };
            }
        }
    }
}
