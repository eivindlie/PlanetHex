using UnityEngine;

namespace Behaviours.Planet.Blocks
{
    public abstract class RenderedBlock : Block
    {
        public override bool IsRendered => true;
        public virtual Material Material => null;
    }
}
