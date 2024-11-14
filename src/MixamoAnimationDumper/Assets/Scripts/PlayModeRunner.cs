using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class PlayModeRunner
{
    [MenuItem("Tools/RunPlayMode")]
    public static void RunPlayMode()
    {
        string scenePath = "Assets/Scenes/SampleScene.unity";
        SetPlayModeStartScene(scenePath);
        if (!EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = true;
            Debug.Log("Unity is now in Play Mode.");
        }
        else
        {
            Debug.Log("Unity is already in Play Mode.");
        }
    }
    public static void SetPlayModeStartScene(string scenePath)
    {
        SceneAsset myWantedStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        if (myWantedStartScene != null)
            EditorSceneManager.playModeStartScene = myWantedStartScene;
        else
            Debug.Log("Could not find Scene " + scenePath);
    }
}