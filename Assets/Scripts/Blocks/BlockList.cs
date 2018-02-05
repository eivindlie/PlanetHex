
using System.Collections.Generic;

namespace Blocks
{
    public class BlockList
    {
        public static Block[] Blocks;

        static BlockList()
        {
            Blocks = new Block[256];
            Blocks[0] = new Air();
            Blocks[1] = new Bedrock();
            Blocks[2] = new Checkered();
        }
    }
}
