using UnityEngine;

public class PersistentDataPathLogger
{
    [UnityEditor.MenuItem("Assets/Print Persistent Data Path")]
    public static void PrintPersistentDataPath()
    {
        Debug.Log(Application.persistentDataPath);
    }
}
