using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    private Color color = new Color(
        237f / 255f,    // red
        200f / 255f,   // bule
        198f / 255f    // green   
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

