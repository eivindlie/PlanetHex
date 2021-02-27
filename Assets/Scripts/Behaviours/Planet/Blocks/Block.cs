namespace Behaviours.Planet.Blocks
{
    public abstract class Block
    {
        public virtual bool IsSolid => false;
        public virtual bool IsRendered => false;
    }
}
