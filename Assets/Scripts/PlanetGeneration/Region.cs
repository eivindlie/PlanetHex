using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PlanetGeneration
{
    public class Region
    {
        public Vector3 Center;
        private List<Tile> Tiles;
        public Chunk[] Chunks;
        public int ID;

        public Region(int id)
        {
            this.ID = id;
            this.Tiles = new List<Tile>();
        }

        public void GenerateTerrain(SimplexNoiseGenerator noiseGenerator, int terrainHeight, int heightLimit, int planetRadius)
        {
            var chunkHeight = Mathf.CeilToInt(heightLimit / Chunk.CHUNK_HEIGHT);
            Chunks = new Chunk[chunkHeight];
            for (var i = 0; i < chunkHeight; i++) Chunks[i] = new Chunk(Center + Center.normalized * Chunk.CHUNK_HEIGHT * i, Tiles.Count, i, this);

            for(var i = 0; i < Tiles.Count; i++) {
                var tile = Tiles[i];
                var height = Mathf.Max(terrainHeight - noiseGenerator.getDensity(tile.Center.AsVector(), 0, terrainHeight, octaves: 3, persistence: 0.60f, multiplier: planetRadius / 2), 0);

                for(var j = 0; j < Chunks.Length; j++)
                {
                    for (var h = 0; h < Chunk.CHUNK_HEIGHT; h++)
                    {
                        if(j * Chunk.CHUNK_HEIGHT + h < height)
                        {
                            Chunks[j].SetBlock(i, h, 1, false);
                        }
                        else
                        {
                            Chunks[j].SetBlock(i, h, 0, false);
                        }
                    }
                    Chunks[j].CheckForEmptiness();
                }
            }
        }

        public void CalculateCenter()
        {
            var x = Tiles.Select(t => t.Center.x).Average();
            var y = Tiles.Select(t => t.Center.y).Average();
            var z = Tiles.Select(t => t.Center.z).Average();

            Center = new Vector3(x, y, z);
        }

        public void AddTile(Tile tile)
        {
            Tiles.Add(tile);
        }

        public List<Tile> GetTiles()
        {
            return Tiles;
        }
    }
}
 