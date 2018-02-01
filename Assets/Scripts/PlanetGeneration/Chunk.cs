using UnityEngine;
using System.Collections.Generic;

namespace PlanetGeneration
{
    public class Chunk
    {
        public Vector3 Center;
        public int[,] Blocks;
        public const int CHUNK_HEIGHT = 10;
        public int ID;
        public Region ParentRegion;

        public Chunk(Vector3 center, int numTiles, int ID, Region parentRegion)
        {
            this.Center = center;
            this.Blocks = new int[numTiles,CHUNK_HEIGHT];
            this.ID = ID;
            this.ParentRegion = parentRegion;
        }
    }
}
