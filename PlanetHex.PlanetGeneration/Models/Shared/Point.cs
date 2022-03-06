using Microsoft.Xna.Framework;

namespace PlanetHex.PlanetGeneration.Models.Shared
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3 AsVector()
        {
            return new Vector3((float)X, (float)Y, (float)Z);
        }
    }
}
