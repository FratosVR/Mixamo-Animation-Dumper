using UnityEditor;

/// <summary>
/// Class in order to close the editor.
/// </summary>
public class EditorCloser
{
    /// <summary>
    /// Closes the editor.
    /// </summary>
    [MenuItem("Tools/CloseEditor")]
    public static void CloseEditor()
    {
        EditorApplication.Exit(0); // 0 indicates a successful exit
    }
}