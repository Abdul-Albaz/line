using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class readJson : MonoBehaviour
{
    // Start is called before the first frame update
    

    public TextAsset JsonFile;

    [System.Serializable]
    public class Colors2
    {
        //employees is case sensitive and must match the string "employees" in the JSON.
        public Colors2[] colors2;
    }

    [System.Serializable]
    public class color2
    {
        public int bgColor;
        public int gridColor;
        public int BallColor;
    }

    

    void Start()
    {
        Colors2 colorsinJsion  = JsonUtility.FromJson<Colors2>(JsonFile.text);

        
    }

    
}
