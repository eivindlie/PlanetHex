using UnityEngine;

namespace Models.Shared
{
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3 AsVector()
        {
            return new Vector3(X, Y, Z);
        }
    }
}
