using UnityEngine;

namespace MathHelpers
{
    public static class SphereHelpers
    {
        public static float DistanceBetweenPoints(Vector3 point1, Vector3 point2, float radius)
        {
            return radius * Mathf.Acos(Vector3.Dot(point1, point2) / Mathf.Pow(radius, 2));
        }
    }
}
