using UnityEngine;

namespace MathHelpers
{
    public static class TriangleNumbers
    {
        public static int CalculateSideLength(int triangleNumber)
        {
            return (int) ((Mathf.Sqrt(8 * triangleNumber + 1) - 1) / 2);
        }

        public static int CalculateTriangleNumber(int sideLength)
        {
            return sideLength * (sideLength + 1) / 2;
        }
    }
}
