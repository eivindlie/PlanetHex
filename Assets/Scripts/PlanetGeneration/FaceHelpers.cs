using Models.HexSphere;

namespace PlanetGeneration
{
    public static class FaceHelpers
    {
        public static bool AreAdjacent(Face face1, Face face2)
        {
            var count = 0;
            foreach (var p1 in face1.Points)
            {
                foreach (var p2 in face2.Points)
                {
                    if (p1 == p2)
                    {
                        count++;
                    }
                }
            }
            return count == 2;
        }
    }
}
