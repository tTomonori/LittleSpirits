//using UnityEditor;
//using UnityEngine;

//public class Importer : AssetPostprocessor
//{
//    const int PostProcessOrder = 0;

//    public override int GetPostprocessOrder()
//    {
//        return PostProcessOrder;
//    }

//    void OnPreprocessTexture()
//    {
//        var textureImporter = assetImporter as TextureImporter;

//        textureImporter.filterMode = FilterMode.Bilinear;
//    }
//}