using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trilController : MonoBehaviour
{
    static public trilController instance;
    public TrailRenderer trail;

    public bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void disableTrail()
    {
        if (trail == null)
            return;


        if (isMoving)
        {

        trail.emitting=false;
        }
    }
}
