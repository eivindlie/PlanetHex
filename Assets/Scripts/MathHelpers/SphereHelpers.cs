using UnityEngine;

namespace MathHelpers
{
    public static class SphereHelpers
    {
        public static float DistanceBetweenPoints(Vector3 point1, Vector3 point2, float radius)
        {
            var p1 = ProjectToRadius(point1, radius);
            var p2 = ProjectToRadius(point2, radius);
            
            return radius * Mathf.Acos(Vector3.Dot(p1, p2) / Mathf.Pow(radius, 2));
        }

        private static Vector3 ProjectToRadius(Vector3 point, float radius)
        {
            var mag = point.magnitude;
            var ratio = radius / mag;

            return point * ratio;
        }
    }
}
