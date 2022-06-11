using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallType : MonoBehaviour {
    
    public Sprite ball1;
    public Sprite ball2;
    public Sprite ball3;
    
    void Start() {
        if(PlayerPrefs.GetInt("BallType") == 0) {
            GetComponent<SpriteRenderer> ().sprite = ball1;
        }else if(PlayerPrefs.GetInt("BallType") == 1) {
            GetComponent<SpriteRenderer> ().sprite = ball2;
        }else if(PlayerPrefs.GetInt("BallType") == 2) {
            GetComponent<SpriteRenderer> ().sprite = ball3;
        }
    }
}
