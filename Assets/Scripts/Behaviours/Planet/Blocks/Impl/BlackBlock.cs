using UnityEngine;

namespace Behaviours.Planet.Blocks.Impl
{
    public class BlackBlock : SolidBlock
    {
        public override Material Material => new Material(Shader.Find("Specular"));
    }
}
