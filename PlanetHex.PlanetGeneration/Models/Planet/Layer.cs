namespace PlanetHex.PlanetGeneration.Models.Planet
{
    public class Layer
    {
        public int LayerNumber { get; set; }
        public int[] Blocks { get; set; } = Array.Empty<int>();
    }
}
