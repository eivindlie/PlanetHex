
using System.Collections.Generic;

namespace Models.Planet
{
    public class Planet
    {
        public HexSphere.HexSphere HexSphere { get; set; }
        public List<Region> Regions { get; set; }
    }
}
