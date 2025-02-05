using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class MocapDumper : MonoBehaviour
{
    private bool _recordMode;
    [SerializeField]
    public int _numUser;
    [SerializeField]
    private string _animationString;
    [SerializeField]
    private List<Transform> _bones;
    private StreamWriter csv_file;
    private string csv_path;
    // Format settings for numbers
    NumberFormatInfo nfi;
    // Start is called before the first frame update
    void Start()
    {
        nfi = new CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalSeparator= ".";
        _recordMode = false;
        csv_path = Path.Combine(Application.dataPath, "CSV", _animationString.ToLower() + "_User" + _numUser.ToString() + ".csv");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) {
            _recordMode = !_recordMode;
            if(_recordMode)
                csv_file = new StreamWriter(csv_path, false);
            else
            {
                csv_file.Close();
            }

        }
        if (_recordMode)
        {
            string data = "";
            foreach (Transform t in _bones)
            {
                data += t.position.x.ToString(nfi) + "," + t.position.y.ToString(nfi) + "," + t.position.z.ToString(nfi) + "," + t.rotation.eulerAngles.x.ToString(nfi) + "," + t.rotation.eulerAngles.y.ToString(nfi) + "," + t.rotation.eulerAngles.z.ToString(nfi) + ",";
            }
            data = data.Remove(data.Length - 1);
            csv_file.Write(data + "\n");
        }
    }
}
