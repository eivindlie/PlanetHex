using PlanetHex.PlanetGeneration.Models.Shared;

namespace PlanetHex.PlanetGeneration.Models.HexSphere
{
    public class Tile
    {
        public Point Center { get; set; } = new();
        public List<Point> Boundary { get; set; } = new();
    }
}
