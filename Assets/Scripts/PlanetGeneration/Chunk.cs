using UnityEngine;
using System.Collections.Generic;

public class Chunk
{
    public Vector3 Center;
    public int[,] Blocks;
    public const int CHUNK_HEIGHT = 10;

    public Chunk(Vector3 center, int numTiles)
    {
        this.Center = center;
        this.Blocks = new int[numTiles,CHUNK_HEIGHT];
    }
}
