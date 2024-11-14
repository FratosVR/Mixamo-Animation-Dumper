using UnityEditor;

public class EditorCloser
{
    [MenuItem("Tools/CloseEditor")]
    public static void CloseEditor()
    {
        EditorApplication.Exit(0); // 0 indicates a successful exit
    }
}