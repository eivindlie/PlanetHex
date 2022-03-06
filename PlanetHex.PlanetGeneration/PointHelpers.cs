using PlanetHex.PlanetGeneration.Models.HexSphere;
using PlanetHex.PlanetGeneration.Models.Shared;

namespace PlanetHex.PlanetGeneration
{
    public static class PointHelpers
    {
        public static Point ProjectToRadius(Point point, float radius, float scale = 1.0f)
        {
            scale = Math.Max(0, Math.Min(1, scale));
            
            var magnitude = Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2) + Math.Pow(point.Z, 2));
            var ratio = radius / magnitude;

            return new Point
            {
                X = point.X * ratio * scale,
                Y = point.Y * ratio * scale,
                Z = point.Z * ratio * scale
            };
        }
        
        public static HexSpherePoint ProjectToRadius(HexSpherePoint point, float radius, float scale=1.0f)
        {
            var projectedPoint = ProjectToRadius(point as Point, radius, scale);

            return new HexSpherePoint
            {
                X = projectedPoint.X,
                Y = projectedPoint.Y,
                Z = projectedPoint.Z,
                Region = point.Region,
                Faces = point.Faces
            };
        }
        
        public static List<Face> GetOrderedFaces(HexSpherePoint point)
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
