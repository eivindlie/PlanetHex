using System.Collections.Generic;

using PlanetGeneration;

using UnityEngine;

namespace Models.HexSphere
{
    public class Point
    {
        public float X { get; internal set; }
        public float Y { get; internal set; }
        public float Z { get; internal set; }
        
        public int? Region { get; internal set; }

        public List<Face> Faces { get; } = new List<Face>();

        public Point(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point() {}

        public void RegisterFace(Face face)
        {
            Faces.Add(face);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point)) return false;

            var p = (Point) obj;

            return Mathf.Abs(X - p.X) < 0.01 && Mathf.Abs(Y - p.Y) < 0.01 && Mathf.Abs(Z - p.Z) < 0.01;
        }
        
        public override int GetHashCode()
        {
            return (X.ToString("0.0") + Y.ToString("0.0") + Z.ToString("0.0")).GetHashCode();
        }

        public override string ToString()
        {
            return $"Point({X:F2}, {Y:F2}, {Z:F2})";
        }

        public Vector3 AsVector()
        {
            return new Vector3(X, Y, Z);
        }
    }
}
