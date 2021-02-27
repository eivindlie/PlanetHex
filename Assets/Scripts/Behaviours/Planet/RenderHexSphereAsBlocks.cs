using System.Collections.Generic;

using Meshes;

using PlanetGeneration;

using UnityEngine;
using UnityEngine.Serialization;

namespace Behaviours.Planet
{
    public class RenderHexSphereAsBlocks : MonoBehaviour
    {
        public List<Material> blockMaterials;
        public float sphereRadius = 100;
        public float blockHeight = 10;

        public void Start()
        {
            var hexSphere = new HexSphereGenerator(sphereRadius, 16).Generate();

            foreach (var tile in hexSphere.Tiles)
            {
                for (var i = 0; i < Random.Range(1, 5); i++)
                {
                    var materialIndex = Random.Range(0, blockMaterials.Count); 
                    var material = blockMaterials[materialIndex];
                    var block = BlockGameObjectFactory.Create(tile, sphereRadius + i * blockHeight, material,
                        blockHeight);
                    block.transform.parent = transform;
                }
            }
        }
    }
}
