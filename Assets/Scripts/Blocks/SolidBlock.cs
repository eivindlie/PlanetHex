using UnityEngine;

namespace Blocks
{
    public class SolidBlock : Block
    {
        public SolidBlock()
        {
            Solid = true;
            Rendered = true;
        }
    }
}