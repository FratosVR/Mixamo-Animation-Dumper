using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimImport : MonoBehaviour
{
    private Animator anim;
    private string path;

    private Dictionary<string, List<AnimationClip>> animacionMap;
    protected AnimatorOverrideController animatorOverrideController;

    private int indexAnim;
    private int index;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = animatorOverrideController;
        index = 0;
        indexAnim = -1;
        timer = 0.0f;
        path = Application.dataPath + "/Resources";
        loadClips();
        setClip();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= anim.GetCurrentAnimatorClipInfo(0)[0].clip.length)
        {
            timer = 0.0f;
            //nextAnimation
            setClip();
        }
    }

    private void setClip()
    {
        anim.Play("Default", 0);

        KeyValuePair<string, List<AnimationClip>> val = animacionMap.ElementAt(index);
        indexAnim++;
        if(indexAnim == val.Value.Count)
        {
            index = (index + 1) % animacionMap.Count;
            val = animacionMap.ElementAt(index);
            indexAnim = 0;
        }
        animatorOverrideController["mixamo.com"] = val.Value[indexAnim];

        anim.Play("Default", 0);
    }


    private void loadClips()
    {
        //AnimationClip[] anims = Resources.FindObjectsOfTypeAll<AnimationClip>();
        try
        {
            string[] dir = Directory.GetDirectories(path);
            foreach (string dirName in dir)
            {
                string[] animsPath = Directory.GetFiles(path + "/" + dirName);
                foreach (string animPath in animsPath)
                {
                    AnimationClip anim = Resources.Load<AnimationClip>(animPath);
                    string[] tmp = dirName.Split('/');
                    string tag = tmp[tmp.Length - 1];
                    animacionMap[tag].Add(anim);
                    
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
        //string path = Application.persistentDataPath + "/dataset";
        //try
        //{
        //    // Obtiene los archivos del directorio
        //    string[] dir = Directory.GetDirectories(path); //C:/Users/pablo/AppData/LocalLow/DefaultCompany/MixamoAnimationDumper
        //    foreach(string dirName in dir)
        //    {
        //        string[] anims = Directory.GetFiles(path + "/" + dirName);
        //        foreach(string anim in anims)
        //        {
        //            AssetDatabase.ImportAsset(dirName + "/" + anim, ImportAssetOptions.Default); //No importa archivos externos->BuildPipeline.BuildAssetBundles, importador externo
        //            AssetBundle.LoadFromFile(dirName + "/" + anim); //Carga un AssetBundle
        //        }
        //    }
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine($"Error: {e.Message}");
        //}

    }
}

