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
using System.Globalization;

public class AnimImport : MonoBehaviour
{
    private Animator anim;
    private string path;
    private string header;
    private string csv_path;
    private StreamWriter csv_file;

    private Dictionary<string, List<AnimationClip>> animacionMap;
    protected AnimatorOverrideController animatorOverrideController;

    private int indexAnim;
    private int index;
    private float timer;

    private const string resourcePath = "Assets/Resources/";

    [SerializeField]
    private List<Transform> _bones;

    // Start is called before the first frame update
    void Start()
    {
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        animacionMap = new Dictionary<string, List<AnimationClip>>();
        anim = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = animatorOverrideController;
        index = 0;
        indexAnim = -1;
        timer = 0.0f;
        path = Application.dataPath + "/Resources";
        Directory.CreateDirectory(Path.Combine(Application.dataPath, "CSV"));
        header = createHeader();
        loadClips();
        setClip();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        string data = "";
        foreach (Transform t in _bones)
        {
            data += t.position.x + "," + t.position.y + "," + t.position.z + "," + t.rotation.eulerAngles.x + "," + t.rotation.eulerAngles.y + "," + t.rotation.eulerAngles.z + ",";
        }
        data = data.Remove(data.Length - 1);
        csv_file.Write(data + "\n");


        if (timer >= anim.GetCurrentAnimatorClipInfo(0)[0].clip.length)
        {
            timer = 0.0f;
            csv_file.Close();
            setClip(); //nextAnimation
        }
    }

    private void setClip()
    {
        KeyValuePair<string, List<AnimationClip>> val = animacionMap.ElementAt(index);
        indexAnim++;
        if (indexAnim == val.Value.Count)
        {
            index++;
            if (index == animacionMap.Count)
                EditorApplication.Exit(0);
            val = animacionMap.ElementAt(index);
            indexAnim = 0;
        }
        csv_path = Path.Combine(Application.dataPath, "CSV", val.Key.ToLower() + "_" + indexAnim.ToString() + ".csv");
        csv_file = new StreamWriter(csv_path, false);
        csv_file.WriteLine(header);
        Debug.Log(csv_path);
        animatorOverrideController[val.Value[indexAnim].name] = val.Value[indexAnim];
        anim.runtimeAnimatorController = animatorOverrideController;
        anim.Play("Default", 0, 0f);
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

    private string createHeader()
    {
        string s = "";
        string[] coordinates = { "x", "y", "z" };

        foreach (Transform bone in _bones)
        {
            foreach (string coordinate in coordinates)
            {
                s += bone.name + "_pos" + coordinate + ",";
            }
            foreach (string coordinate in coordinates)
            {
                s += bone.name + "_rot" + coordinate + ",";
            }
        }

        s = s.Remove(s.Length - 1);

        return s;
    }
}