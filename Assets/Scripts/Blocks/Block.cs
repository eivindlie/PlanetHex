using UnityEngine;

namespace Blocks
{
    public abstract class Block
    {
        public Material Material { get; protected set; }
        public bool Solid { get; protected set; }
        public bool Rendered { get; protected set; }
    }
}