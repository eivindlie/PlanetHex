using System.Collections.Generic;

using PlanetGeneration;

using UnityEngine;

namespace Models.HexSphere
{
    public class Point
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
        
        public int? Region { get; set; }

        public List<Face> Faces { get; } = new List<Face>();

        public Point(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

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

        public Point ProjectToRadius(float radius, float scale=1.0f, bool mutate=false)
        {
            scale = Mathf.Max(0, Mathf.Min(1, scale));
            
            var magnitude = Mathf.Sqrt(Mathf.Pow(X, 2) + Mathf.Pow(Y, 2) + Mathf.Pow(Z, 2));
            var ratio = radius / magnitude;
            
            var x = X * ratio * scale;
            var y = Y * ratio * scale;
            var z = Z * ratio * scale;

            if (mutate)
            {
                X = x;
                Y = y;
                Z = z;
                return this;
            }
            return new Point(x, y, z);
        }
        
        public List<Face> GetOrderedFaces()
        {
            var workingList = new List<Face>(Faces);
            var ret = new List<Face>();

            var i = 0;
            while (i < Faces.Count)
            {
                if (i == 0)
                {
                    ret.Add(workingList[i]);
                    workingList.RemoveAt(i);
                }
                else
                {
                    for(var j = 0; j < workingList.Count; j++)
                    {
                        if (i > ret.Count || !FaceHelpers.AreAdjacent(workingList[j], ret[i - 1])) continue;
                        
                        ret.Add(workingList[j]);
                        workingList.RemoveAt(j);
                        break;
                    }
                }
                i++;
            }
            return ret;
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
