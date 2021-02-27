using System.Collections.Generic;

using Models.Shared;

namespace Models.Planet
{
    public class Region
    {
        public Point Center { get; set; }
        public Chunk[] Chunks { get; set; }
    }
}
