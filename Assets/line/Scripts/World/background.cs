using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    private Color color = new Color(
        255f / 255f,    // red
        255f / 255f,   // bule
        255f / 255f    // green   
                   );    

    public Camera cm;

    void Start()
    {
        cm = GetComponent<Camera>();
       
    }

    void Update()
    {
        
     cm.backgroundColor = color;
        
    }
}

