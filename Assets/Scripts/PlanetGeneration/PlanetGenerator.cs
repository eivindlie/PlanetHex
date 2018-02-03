using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Utilities;

namespace PlanetGeneration
{


    public class PlanetGenerator : MonoBehaviour
    {
        public int Radius;
        public int TerrainHeight;
        public int HeightLimit;
        private int NumDivisions;

        public GameObject Player;
        public Material[] Materials;
        public Region[] Regions;

        private GameObject PlanetSphere;
        private PriorityQueue<Chunk> ChunkQueue;
        private SimplexNoiseGenerator NoiseGenerator = new SimplexNoiseGenerator();
        private Hexasphere Hexasphere;
        private Dictionary<Chunk, GameObject> LoadedChunks;
        private IEnumerator RegionLoader;

        void Start()
        {
            NumDivisions = (int)Mathf.Sqrt((Mathf.PI * Mathf.Pow(Radius, 2)) / 5);
            Debug.Log("Subdivisions: " + NumDivisions);

            HeightLimit = TerrainHeight + 20;
            Hexasphere = new Hexasphere(Radius, NumDivisions);
            Regions = Hexasphere.Regions;
            foreach (var region in Regions)
            {
                region.GenerateTerrain(NoiseGenerator, TerrainHeight, HeightLimit, Radius);
            }

            LoadedChunks = new Dictionary<Chunk, GameObject>();
            ChunkQueue = new PriorityQueue<Chunk>();
            PlanetSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            PlanetSphere.name = "PlanetSphere";
            PlanetSphere.transform.localScale = new Vector3(2*Radius, 2*Radius, 2*Radius);
            PlanetSphere.transform.parent = transform;

            RegionLoader = LoadRegions();
            StartCoroutine(RegionLoader);
        }

        void Update()
        {
            var pos = Player.transform.position;//(player.transform.position - this.transform.position).normalized * Radius;

            foreach (var region in Regions)
            {
                foreach(var chunk in region.Chunks)
                {
                    var distance = (chunk.Center - pos).magnitude;
                    if (distance < (Radius / Hexasphere.RegionDivisions) * 1.2)
                    {
                        if (!LoadedChunks.ContainsKey(chunk))
                        {
                            LoadedChunks[chunk] = null;
                            ChunkQueue.Enqueue(chunk, distance);
                        }
                    }
                    else if (distance > (Radius / Hexasphere.RegionDivisions) * 2) // Don't unload closest regions - player might turn
                    {
                        if (LoadedChunks.ContainsKey(chunk))
                        {
                            UnloadChunk(chunk);
                        }
                    }
                }
            }
        }

        IEnumerator LoadRegions()
        {
            var priorityThreshold = Mathf.Sqrt(Hexasphere.Regions[0].GetTiles().Count) * 3;

            while(true)
            {
                if (ChunkQueue.IsEmpty()) { yield return null; continue; }

                var priority = ChunkQueue.PeekAtPriority();
                var tilesPerFrame = 1000; //(priority < priorityThreshold) ? 200 : 1;

                var chunk = ChunkQueue.Dequeue();

                var regionTransform = transform.Find("region_" + chunk.ParentRegion.ID);
                GameObject regionGameObject;
                if (regionTransform == null)
                {
                    regionGameObject = new GameObject("region_" + chunk.ParentRegion.ID);
                    regionGameObject.transform.parent = this.transform;
                }
                else
                {
                    regionGameObject = regionTransform.gameObject;
                }
                var chunkGameObject = new GameObject("chunk_" + chunk.ID);
                chunkGameObject.transform.parent = regionGameObject.transform;

                var i = 0;
                for (var j = 0; j < chunk.ParentRegion.GetTiles().Count; j++)
                {
                    var tile = chunk.ParentRegion.GetTiles()[j];
                    for (var h = 0; h < Chunk.CHUNK_HEIGHT; h++)
                    {
                        if(chunk.GetBlock(j,h) != 0)
                        {
                            var block = CreateBlock(tile, chunk.ID * Chunk.CHUNK_HEIGHT + h, "block_" + j + "_" + h);
                            block.transform.parent = chunkGameObject.transform;
                        }
                    }
                    i++;
                    if(i > tilesPerFrame)
                    {
                        i = 0;
                        yield return null;
                    }
                }
                LoadedChunks[chunk] = chunkGameObject;
            }

        }

        void UnloadChunk(Chunk chunk)
        {
            if (LoadedChunks.ContainsKey(chunk))
            {
                Destroy(LoadedChunks[chunk]);
                LoadedChunks.Remove(chunk);
            }
        }

        public void RemoveBlock(GameObject block, bool removeNonDestructible = false)
        {
            try
            {
                var block_index = int.Parse(block.name.Split('_')[1]);
                var block_height = int.Parse(block.name.Split('_')[2]);
                var chunk_index = int.Parse(block.transform.parent.gameObject.name.Split('_')[1]);
                var region_index = int.Parse(block.transform.parent.parent.gameObject.name.Split('_')[1]);
                if (removeNonDestructible || Regions[region_index].Chunks[chunk_index].GetBlock(block_index, block_height) != 1)
                {
                    Regions[region_index].Chunks[chunk_index].SetBlock(block_index, block_height, 0);
                    Destroy(block);
                }
            }
            catch(IndexOutOfRangeException e)
            {
                return;
            }
        }

        GameObject CreateBlock(Tile tile, int height, string name = "Block")
        {
            var block = new GameObject(name);
            block.AddComponent<MeshFilter>();
            block.AddComponent<MeshRenderer>();
            block.AddComponent<MeshCollider>();

            var mesh = CreateMesh(tile, height);
            block.GetComponent<MeshFilter>().mesh = mesh;

            var meshCollider = block.GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;

            var meshRenderer = block.GetComponent<MeshRenderer>();
            if (tile.Region != -1)
            {
                meshRenderer.material = Materials[tile.Region % Materials.Length];
            }

            block.transform.position = tile.Center.Project(Radius + height).AsVector();
            return block;
        }

        Mesh CreateMesh(Tile tile, int height = 0)
        {
            var count = tile.Boundary.Count;
            var offset = count * 2 - 1;


            var vertices = new Vector3[offset * 2];
            var tris = new List<int>();
            var pos = tile.Center.Project(Radius + height).AsVector();
            var vertexIndex = 0;
            for (var i = 0; i < tile.Boundary.Count; i++)
            {
                vertices[vertexIndex] = tile.Boundary[i].Project(Radius + height).AsVector() - pos;
                vertices[offset + vertexIndex] = tile.Boundary[i].Project(Radius + height + 1).AsVector() - pos;
                var next = 1;
                if (i != 1 && i != 2)
                {
                    next++;
                    vertices[vertexIndex + 1] = tile.Boundary[i].Project(Radius + height).AsVector() - pos;
                    vertices[offset + vertexIndex + 1] = tile.Boundary[i].Project(Radius + height + 1).AsVector() - pos;
                }
                if (i == 0)
                {
                    next++;
                    vertices[vertexIndex + 2] = tile.Boundary[i].Project(Radius + height).AsVector() - pos;
                    vertices[offset + vertexIndex + 2] = tile.Boundary[i].Project(Radius + height + 1).AsVector() - pos;
                }
                if (i == count - 1)
                {
                    next += 2;
                }


                tris.Add(vertexIndex); tris.Add((vertexIndex + next) % offset); tris.Add(vertexIndex + offset);
                tris.Add((vertexIndex + next) % offset); tris.Add((vertexIndex + next) % offset + offset); tris.Add(vertexIndex + offset);
                vertexIndex += next;
            }

            tris.Add(1); tris.Add(4); tris.Add(3);
            tris.Add(offset + 1); tris.Add(offset + 3); tris.Add(offset + 4);
            tris.Add(4); tris.Add(8); tris.Add(6);
            tris.Add(offset + 4); tris.Add(offset + 6); tris.Add(offset + 8);
            tris.Add(8); tris.Add(4); tris.Add(1);
            tris.Add(offset + 8); tris.Add(offset + 1); tris.Add(offset + 4);

            if (tile.Boundary.Count > 5)
            {
                tris.Add(8); tris.Add(1); tris.Add(10);
                tris.Add(offset + 8); tris.Add(offset + 10); tris.Add(offset + 1);
            }

            Vector2[] uvs = null;
            if (count == 5)
            {
                uvs = new Vector2[] {
                    new Vector2(1, 0.3078f),
                    new Vector2(0.8617f, 0.1176f),
                    new Vector2(0, 0.3078f),
                    new Vector2(0.8f, 0.3078f),
                    new Vector2(0.6f, 0.3078f),
                    new Vector2(0.4f, 0.3078f),
                    new Vector2(0.5382f, 0.1176f),
                    new Vector2(0.2f, 0.3078f),
                    new Vector2(0.6999f, 0),
                    new Vector2(1, 0.4778f),
                    new Vector2(0.8617f, 0.6680f),
                    new Vector2(0, 0.4778f),
                    new Vector2(0.8f, 0.4778f),
                    new Vector2(0.6f, 0.4778f),
                    new Vector2(0.4f, 0.4778f),
                    new Vector2(0.5382f, 0.6680f),
                    new Vector2(0.2f, 0.4778f),
                    new Vector2(0.6999f, 0.7856f),
                };
            }
            else if (count == 6)
            {
                uvs = new Vector2[]
                {
                    new Vector2(1, 0.2887f),
                    new Vector2(0.9166f, 0.1444f),
                    new Vector2(0, 0.2887f),
                    new Vector2(0.8332f, 0.2887f),
                    new Vector2(0.6666f, 0.2887f),
                    new Vector2(0.5f, 0.2887f),
                    new Vector2(0.5833f, 0.1444f),
                    new Vector2(0.3333f, 0.2887f),
                    new Vector2(0.6666f, 0),
                    new Vector2(0.1667f, 0.2887f),
                    new Vector2(0.8332f, 0),
                    new Vector2(1, 0.4553f),
                    new Vector2(0.9166f, 0.5996f),
                    new Vector2(0, 0.4553f),
                    new Vector2(0.8332f, 0.4553f),
                    new Vector2(0.6666f, 0.4553f),
                    new Vector2(0.5f, 0.4553f),
                    new Vector2(0.5833f, 0.5996f),
                    new Vector2(0.3333f, 0.4453f),
                    new Vector2(0.6666f, 0.7439f),
                    new Vector2(0.1667f, 0.4553f),
                    new Vector2(0.8332f, 0.7436f),
                };
            }

            var mesh = new Mesh()
            {
                vertices = vertices,
                triangles = tris.ToArray(),
                uv = uvs
            };
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}

