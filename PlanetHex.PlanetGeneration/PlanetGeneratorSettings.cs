namespace PlanetHex.PlanetGeneration
{
    public class PlanetGeneratorSettings
    {
        public int BaseRadius { get; set; } = 100;
        public int HeightLimit { get; set; } = 20;
        public int ChunkHeight { get; set; } = 15;
        public int BlockHeight { get; set; } = 1;
        
        public int Divisions { get; set; } = 16;
    }
}
