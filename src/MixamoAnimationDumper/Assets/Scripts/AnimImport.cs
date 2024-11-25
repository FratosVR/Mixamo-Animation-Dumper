using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

    private const string resourcePath = "Assets/Resources/";

    // Start is called before the first frame update
    void Start()
    {
        animacionMap = new Dictionary<string, List<AnimationClip>>();
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
        //timer += Time.deltaTime;
        //if (timer >= anim.GetCurrentAnimatorClipInfo(0)[0].clip.length)
        //{
        //    timer = 0.0f;
        //    //nextAnimation
        //    setClip();
        //}
    }

    private void setClip()
    {
        KeyValuePair<string, List<AnimationClip>> val = animacionMap.ElementAt(index);
        indexAnim++;
        if(indexAnim == val.Value.Count)
        {
            index = (index + 1) % animacionMap.Count;
            val = animacionMap.ElementAt(index);
            indexAnim = 0;
        }
        animatorOverrideController[val.Value[indexAnim].name] = val.Value[indexAnim];
        anim.runtimeAnimatorController = animatorOverrideController;
        anim.Play("Default", 0);
    }


    private void loadClips()
    {
        try
        {
            string[] dir = Directory.GetDirectories(path);
            foreach (string dirName in dir)
            {
                string[] animsPath = Directory.GetFiles(dirName);
                string[] tmp = dirName.Split('\\');
                animacionMap.Add(tmp[tmp.Length - 1], new List<AnimationClip>());
                foreach (string animPath in animsPath)
                {
                    //var asset = Resources.Load<GameObject>(animPath);
                    //ModelImporterClipAnimation anim = asset.defaultClipAnimations[0];
                    //AnimationClip anim = AnimationUtility.GetAnimationClips(asset)[0];
                    //string[] tmp = dirName.Split('/');

                    string[] tmp2 = animPath.Split('\\');
                    string a = Path.Combine(resourcePath, tmp2[tmp2.Length - 2], tmp2[tmp2.Length - 1]);
                    var allAsset = AssetDatabase.LoadAllAssetsAtPath(Path.Combine(resourcePath, tmp2[tmp2.Length - 2], tmp2[tmp2.Length - 1]));
                    foreach (var asset in allAsset)
                    {
                        if (asset is AnimationClip && asset.name != "__preview__mixamo.com")
                        {
                            //Debug.Log(asset.name);
                            string tag = tmp[tmp.Length - 1];
                            animacionMap[tag].Add(asset as AnimationClip);
                            break;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error: {e.Message}");
        }
    }
}

