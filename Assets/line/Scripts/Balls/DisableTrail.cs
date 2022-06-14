using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTrail : MonoBehaviour
{

    public GameObject trail;
    public TrailRenderer line;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (trail == null)
        {
            Debug.Log("no trail");
            return;
        }

        else if (Vars.isBallMoving == true)
        {
            line.time = 2.0f;
            


        }

        else if (Vars.isBallMoving == false)
        {

            //Invoke("disLine", 2.0f);
  
        }

    }


void disLine()
    {
        line.time = 0f;

        CancelInvoke();
    }
   
}
