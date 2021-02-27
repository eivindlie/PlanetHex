using Models.HexSphere;

using UnityEngine;

namespace Meshes
{
    public static class BlockGameObjectFactory
    {
        public static GameObject Create(Tile tile, float bottomRadius, Material material, float height = 1.0f)
        {
            var block = new GameObject("HexBlock");
            block.AddComponent<MeshFilter>();
            block.AddComponent<MeshRenderer>();
            block.AddComponent<MeshCollider>();

            var mesh = TileToMeshMapper.Map(tile, bottomRadius, height);
            block.GetComponent<MeshFilter>().mesh = mesh;
            
            block.GetComponent<MeshCollider>().sharedMesh = mesh;
            
            block.GetComponent<MeshRenderer>().material = material;

            block.transform.position = tile.Center.ProjectToRadius(bottomRadius).AsVector();
            return block;
        }
    }
}
