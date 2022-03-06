namespace PlanetHex.PlanetGeneration.Models.Planet
{
    public class Chunk
    {
        public int ChunkNumber { get; set; }
        public Layer[] Layers { get; set; } = Array.Empty<Layer>();

        // public Block GetBlock(int layer, int index)
        // {
        //     return null;
        // }
    }
}
