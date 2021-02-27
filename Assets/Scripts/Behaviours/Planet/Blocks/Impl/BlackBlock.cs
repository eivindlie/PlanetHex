using UnityEngine;

namespace Behaviours.Planet.Blocks.Impl
{
    public class BlackBlock : SolidBlock
    {
        public const int BlockId = 1;

        public BlackBlock()
        {
            Material = Resources.Load<Material>("Materials/Black");
        }
    }
}
