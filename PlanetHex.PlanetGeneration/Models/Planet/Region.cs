using PlanetHex.PlanetGeneration.Models.Shared;

namespace PlanetHex.PlanetGeneration.Models.Planet
{
    public class Region
    {
        public int RegionNumber { get; set; }
        public Point Center { get; set; } = new();
        public Chunk[] Chunks { get; set; } = Array.Empty<Chunk>();
    }
}
