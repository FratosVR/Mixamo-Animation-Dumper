using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

/// <summary>
/// Class that plays the scene in the editor.
/// </summary>
public class PlayModeRunner
{
    /// <summary>
    /// Runs the scene.
    /// </summary>
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
    /// <summary>
    /// Sets the scene that will be played.
    /// </summary>
    /// <param name="scenePath"></param>
    public static void SetPlayModeStartScene(string scenePath)
    {
        SceneAsset myWantedStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        if (myWantedStartScene != null)
            EditorSceneManager.playModeStartScene = myWantedStartScene;
        else
            Debug.Log("Could not find Scene " + scenePath);
    }
}