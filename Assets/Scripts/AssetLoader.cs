using UnityEngine;
using System.Collections;
using System.IO;
using Blocks;

public class AssetLoader : MonoBehaviour
{

    public static AssetBundle blockMaterialBundle;

    // Use this for initialization
    void Start()
    {
        blockMaterialBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles/blocks"));
        new BlockList();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
