
namespace PlanetHex.PlanetGeneration.Models.Planet
{
    public class Planet
    {
        public int BlockHeight { get; set; } = 1;
        public int ChunkHeight { get; set; } = 15;
        public int BaseRadius { get; set; } = 100;
        public int HeightLimit { get; set; } = 20;
        public HexSphere.HexSphere HexSphere { get; set; } = new();
        public Region[] Regions { get; set; } = Array.Empty<Region>();
    }
}
