using System.Collections.Generic;

using Behaviours.Planet.Blocks;

using Extensions;

using Models.Planet;

using PlanetGeneration;

using UnityEngine;

namespace Rendering
{
    public static class ChunkRenderer
    {
        public static List<GameObject> Render(Planet planet, int regionIndex, int chunkIndex)
        {
            var tileRegion = planet.HexSphere.Regions[regionIndex];
            var chunk = planet.Regions[regionIndex].Chunks[chunkIndex];

            var gameObjects = new List<GameObject>();
            
            foreach (var (layer, layerIndex) in chunk.Layers.WithIndex())
            {
                foreach (var (blockId, index) in layer.Blocks.WithIndex())
                {
                    var tile = tileRegion.Tiles[index];
                    var block = BlockMap.GetBlock(blockId);

                    var bottomRadius = planet.BaseRadius +
                                       (chunkIndex * planet.ChunkHeight + layerIndex) * planet.BlockHeight;

                    if (!block.IsRendered) continue;

                    var mesh = TileToMeshMapper.Map(tile,
                        bottomRadius,
                        planet.BlockHeight);

                    var go = new GameObject("Block");
                    go.AddComponent<MeshFilter>();
                    go.AddComponent<MeshRenderer>();
                    go.GetComponent<MeshFilter>().mesh = mesh;
                    go.GetComponent<MeshRenderer>().material = (block as RenderedBlock)?.Material;

                    if (block.IsSolid)
                    {
                        go.AddComponent<MeshCollider>();
                        go.GetComponent<MeshCollider>().sharedMesh = mesh;
                    }

                    go.transform.position = PointHelpers.ProjectToRadius(tile.Center, bottomRadius).AsVector();
                    
                    gameObjects.Add(go);
                }
            }
            
            return gameObjects;
        }
    }
}
