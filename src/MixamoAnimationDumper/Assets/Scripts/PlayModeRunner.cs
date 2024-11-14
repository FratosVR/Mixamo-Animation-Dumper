using UnityEditor;
using UnityEngine;

public class PlayModeRunner
{
    [MenuItem("Tools/RunPlayMode")]
    public static void RunPlayMode()
    {
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
}