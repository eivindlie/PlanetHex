using UnityEngine;

namespace Blocks
{
    public class Checkered : SolidBlock
    {
        public Checkered()
        {
            Material = AssetLoader.blockMaterialBundle.LoadAsset("Checkered", typeof(Material)) as Material;
        }
    }
}