using UnityEngine;

namespace Behaviours.Planet.Blocks.Impl
{
    public class BlackBlock : SolidBlock
    {
        public const int BlockId = 1;
        public override Material Material => new Material(Shader.Find("Specular"));
    }
}
