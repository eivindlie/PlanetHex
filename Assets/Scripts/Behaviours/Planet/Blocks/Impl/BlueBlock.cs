using UnityEngine;

namespace Behaviours.Planet.Blocks.Impl
{
    public class BlueBlock : SolidBlock
    {
        public const int BlockId = 2;

        public BlueBlock()
        {
            Material = Resources.Load<Material>("Materials/Black");
        }
    }
}
