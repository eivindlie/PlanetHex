using System.Collections.Generic;

using Models.HexSphere;

using UnityEngine;

namespace PlanetGeneration
{
    public static class PointHelpers
    {
        public static Point ProjectToRadius(Point point, float radius, float scale=1.0f, bool mutate=false)
        {
            scale = Mathf.Max(0, Mathf.Min(1, scale));
            
            var magnitude = Mathf.Sqrt(Mathf.Pow(point.X, 2) + Mathf.Pow(point.Y, 2) + Mathf.Pow(point.Z, 2));
            var ratio = radius / magnitude;
            
            var x = point.X * ratio * scale;
            var y = point.Y * ratio * scale;
            var z = point.Z * ratio * scale;

            if (!mutate) return new Point
            {
                X = x,
                Y = y,
                Z = z,
                Region = point.Region
            };
            
            point.X = x;
            point.Y = y;
            point.Z = z;
            return point;
        }
        
        public static List<Face> GetOrderedFaces(Point point)
        {
            var workingList = new List<Face>(point.Faces);
            var ret = new List<Face>();

            var i = 0;
            while (i < point.Faces.Count)
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
    }
}
