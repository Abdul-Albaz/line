using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTrail : MonoBehaviour
{

    public GameObject trail;
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
            trail.SetActive(true);
            
        }

        else if (Vars.isBallMoving == false)
        {

            Invoke("disLine", 0.2f);

           
        }


    }


void disLine()
    {
        trail.SetActive(false);
        CancelInvoke();
    }
   
}
