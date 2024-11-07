
using System.Drawing;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleCreator
{
    [MenuItem("Assets/Build Asset Bundles")]
    public static void BuildAllAssetBundles()
    {
        string outputPath = Application.persistentDataPath + "/AssetBundles/";
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        // Load .obj files and mark for asset bundle creation
        string persistentObjFolderPath = Application.dataPath + "/ObjFiles/";
        if(!Directory.Exists(persistentObjFolderPath))
        {
            Directory.CreateDirectory(persistentObjFolderPath);
        }

        string objFolderPath = Application.dataPath + "/objFiles/";

        //foreach (string file in Directory.GetFiles(objFolderPath))
        //{
        //    File.Copy(file, persistentObjFolderPath + Path.GetFileName(file), true);
        //}

        //DirectoryInfo dir = new DirectoryInfo(persistentObjFolderPath);
        //string assetBundleName = "";
        //foreach (FileInfo file in dir.GetFiles("*.obj"))
        //{

        //    string relativePath = persistentObjFolderPath + file.Name;
        //    assetBundleName = Path.GetFileNameWithoutExtension(file.Name) + ".assetbundle";  // Use the filename without extension as the bundle name

        //    Debug.Log("Processing file: " + relativePath); // Log the file being processed
        //    var importer = AssetImporter.GetAtPath(relativePath);
        //    if (importer == null)
        //    {
        //        Debug.LogError("Failed to get AssetImporter for: " + relativePath);
        //        continue; // Skip to the next file if the importer is null
        //    }

        //    importer.assetBundleName = assetBundleName;
        //    Debug.Log("Set asset bundle name: " + assetBundleName);
        //}

        string assetBundleName = "";
        foreach (string f in Directory.GetFiles(persistentObjFolderPath))
        {
            if(Path.GetExtension(f) == ".obj"){
                string relativePath = objFolderPath + Path.GetFileName(f);
                assetBundleName = Path.GetFileNameWithoutExtension(f) /*+ "_assetbundle"*/;  // Use the filename without extension as the bundle name

                Debug.Log("Processing file: " + relativePath); // Log the file being processed
                var importer = AssetImporter.GetAtPath(relativePath);
                if (importer == null)
                {
                    Debug.LogError("Failed to get AssetImporter for: " + relativePath);
                    continue; // Skip to the next file if the importer is null
                }

                importer.assetBundleName = assetBundleName;
                Debug.Log(importer.assetPath);
                Debug.Log("Set asset bundle name: " + assetBundleName);
            }
        }
        // Build asset bundles
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        Debug.Log("Asset Bundles built successfully at: " + outputPath);

        foreach (string file in Directory.GetFiles(outputPath))
        {
            if (Path.GetExtension(file) != ".manifest" && Path.GetFileNameWithoutExtension(file) != "AssetBundles")
            {
                var myLoadedAssetBundle = AssetBundle.LoadFromFile(file);
                if (myLoadedAssetBundle == null)
                {
                    Debug.Log("Failed to load AssetBundle " + file);
                    return;
                }

                foreach (string s in myLoadedAssetBundle.GetAllAssetNames())
                {
                    Debug.Log(s);
                }

                var loaded = myLoadedAssetBundle.LoadAsset<AnimationClip>(Path.GetFileName(file));

                if (loaded == null)
                {
                    Debug.LogError("Failed to load.");
                }
                Debug.Log(loaded.name);

                myLoadedAssetBundle.Unload(false);
            }
            //File.Delete(file);
        }

        ////outputPath = outputPath + assetBundleName;

        ////AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(outputPath);

        ////Debug.Log(clip.name);
    }
}
