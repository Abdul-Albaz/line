using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuncingBallAnimation : MonoBehaviour
{
    private bool Left = false;
    private float postion = 0.1f;
    // Start is called before the first frame update
    void start()
    {
        Left = false;
        postion = 0.1f;
    }

    void Update()
    {
        if (Left)
        {
            postion += Time.deltaTime ;
            if (postion >= 0.5)
            {
                Left = false;
            }
        }
        else
        {
            postion -= Time.deltaTime ;
            if (postion <= 0.1f)
            {
                Left = true;
            }
        }
        transform.localScale = new Vector2(postion,postion);
    }
}
