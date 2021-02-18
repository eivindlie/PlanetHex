using Meshes;

using PlanetGeneration;

using UnityEngine;

namespace Behaviours.Planet
{
    public class RenderHexSphereAsBlocks : MonoBehaviour
    {
        public Material blockMaterial;
        public float SphereRadius = 100;
        public float BlockHeight = 10;

        public void Start()
        {
            var hexSphere = new HexSphereGenerator(SphereRadius, 16).Generate();

            foreach (var tile in hexSphere.Tiles)
            {
                for (var i = 0; i < Random.Range(1, 5); i++)
                {
                    var block = BlockGameObjectFactory.Create(tile, SphereRadius + i * BlockHeight, blockMaterial,
                        BlockHeight);
                    block.transform.parent = transform;
                }
            }
        }
    }
}
