using System.Collections.Generic;

using Models.Shared;

using UnityEngine;

namespace Models.HexSphere
{
    public class HexSpherePoint : Point
    {
        public int? Region { get; internal set; }

        public List<Face> Faces { get; set; } = new List<Face>();

        public HexSpherePoint(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public HexSpherePoint() { }

        public void RegisterFace(Face face)
        {
            Faces.Add(face);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is HexSpherePoint)) return false;

            var p = (HexSpherePoint) obj;

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

    }
}
