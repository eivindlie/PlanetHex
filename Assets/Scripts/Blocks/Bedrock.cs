using UnityEngine;

namespace Blocks
{
    public class Bedrock : SolidBlock
    {
        public Bedrock()
        {
            Material = AssetLoader.blockMaterialBundle.LoadAsset("Bedrock", typeof(Material)) as Material;
        }
    }
}