using UnityEngine;

namespace Behaviours.Planet.Blocks.Impl
{
    public class GreenBlock : SolidBlock
    {
        public const int BlockId = 3;

        public GreenBlock()
        {
            Material = Resources.Load<Material>("Materials/Green");
        }
    }
}
