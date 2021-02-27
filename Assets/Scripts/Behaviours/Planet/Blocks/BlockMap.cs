using System.Collections.Generic;

using Behaviours.Planet.Blocks.Impl;

namespace Behaviours.Planet.Blocks
{
    public static class BlockMap
    {
        private static Dictionary<int, Block> Map = new Dictionary<int, Block>
        {
            [AirBlock.BlockId] = new AirBlock(),
            [BlackBlock.BlockId] = new BlackBlock(),
            [BlueBlock.BlockId] = new BlueBlock(),
        };

        public static Block GetBlock(int blockId)
        {
            return Map[blockId];
        }
    }
}
