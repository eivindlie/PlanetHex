using System.Collections.Generic;

using Behaviours.Planet.Blocks.Impl;

namespace Behaviours.Planet.Blocks
{
    public static class BlockMap
    {
        private static Dictionary<int, Block> Map = new Dictionary<int, Block>
        {
            [0] = new AirBlock(),
            [1] = new BlackBlock(),
        };

        public static Block GetBlock(int blockId)
        {
            return Map[blockId];
        }
    }
}
