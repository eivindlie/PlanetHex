using System.Collections.Generic;

using Behaviours.Planet.Blocks;

namespace Models.Planet
{
    public class Chunk
    {
        public Layer[] Layers { get; set; }

        public Block GetBlock(int layer, int index)
        {
            return null;
        }
    }
}
