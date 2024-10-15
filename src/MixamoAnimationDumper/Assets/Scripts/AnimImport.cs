using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimImport : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private List<AnimationClip> animaciones;

    private Dictionary<string, AnimationClip[]> animacionMap;
    protected AnimatorOverrideController animatorOverrideController;

    private int index;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        anim = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = animatorOverrideController;
        index = -1;
        timer = 0.0f;
        //setClip();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= anim.GetCurrentAnimatorClipInfo(0)[0].clip.length)
        {
            timer = 0.0f;
            //nextAnimation
        }
    }

    private void setClip()
    {
        anim.Play("Default", 0);
        index = (index + 1) % animaciones.Count;
        animatorOverrideController["mixamo.com"] = animaciones[index];
        anim.Play("Default", 0);
    }


    private void loadClips()
    {
        string path = Application.persistentDataPath + "/dataset";
        try
        {
            // Obtiene los archivos del directorio
            string[] dir = Directory.GetDirectories(path); //C:/Users/pablo/AppData/LocalLow/DefaultCompany/MixamoAnimationDumper
            foreach(string dirName in dir)
            {
                string[] anims = Directory.GetFiles(path + "/" + dirName);
                foreach(string anim in anims)
                {
                    AssetDatabase.ImportAsset(dirName + "/" + anim, ImportAssetOptions.Default); //No importa archivos externos->BuildPipeline.BuildAssetBundles, importador externo
                    AssetBundle.LoadFromFile(dirName + "/" + anim); //Carga un AssetBundle
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    private void CreateAnim(string fbx, string target)
    {
        var fileName = Path.GetFileNameWithoutExtension(fbx);
        var filePath = $"{target}/{fileName}.anim";
        AnimationClip src = AssetDatabase.LoadAssetAtPath<AnimationClip>(fbx);
        AnimationClip temp = new AnimationClip();
        EditorUtility.CopySerialized(src, temp);
        AssetDatabase.CreateAsset(temp, filePath);
        AssetDatabase.SaveAssets();
    }
}

