
using System.Collections.Generic;

namespace Models.Planet
{
    public class Planet
    {
        public int BlockHeight { get; set; } = 1;
        public int ChunkHeight { get; set; } = 15;
        public int BaseRadius { get; set; } = 100;
        public int HeightLimit { get; set; } = 20;
        public HexSphere.HexSphere HexSphere { get; set; }
        public Region[] Regions { get; set; }
    }
}
