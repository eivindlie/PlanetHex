
using System.Collections.Generic;

namespace Models.Planet
{
    public class Planet
    {
        public int BlockHeight { get; set; } = 1;
        public int ChunkHeight { get; set; } = 15;
        public int BaseRadius { get; set; } = 100;
        public HexSphere.HexSphere HexSphere { get; set; }
        public List<Region> Regions { get; set; }
    }
}
