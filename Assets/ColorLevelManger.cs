using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLevelManger : MonoBehaviour
{

    [System.Serializable]
    public class HanelColor
    {
        public int bg;
        public int grid;
        public int ball;


        public static T ImportJson<T>(string path)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("Assets/json Files/LevelColor");
            
            return JsonUtility.FromJson<T>(textAsset.text);
            
        }
    }

    
}
