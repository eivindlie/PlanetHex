namespace PlanetHex.PlanetGeneration.Models.HexSphere
{
    public class Polyhedron
    {
        public List<HexSpherePoint> Corners { get; set; } = new();
        public List<Face> Faces { get; set; } = new();
    }
}
