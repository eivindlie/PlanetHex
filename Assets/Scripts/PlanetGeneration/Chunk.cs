using UnityEngine;
using System.Collections.Generic;

namespace PlanetGeneration
{
    public class Chunk
    {
        public Vector3 Center;
        private int[,] Blocks;
        public bool Empty { get; private set; }
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

        public void SetBlock(int x, int y, int value, bool checkForEmptiness = true)
        {
            if (value != 0) Empty = false;
            else if (Blocks[x, y] == 0 && checkForEmptiness) {
                CheckForEmptiness();
            }
            Blocks[x, y] = value;
        }

        public void CheckForEmptiness()
        {
           Empty = true;
            foreach (var block in Blocks)
            {
                if (block != 0)
                {
                    Empty = false;
                    break;
                }
            }
        }

        public int GetBlock(int x, int y)
        {
            return Blocks[x, y];
        }
    }
}
