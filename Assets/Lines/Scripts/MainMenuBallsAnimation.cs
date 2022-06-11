using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBallsAnimation : MonoBehaviour {
    
    public Image[] balls;
    int counter = 0;
    bool up = false;
    float alpha = 1;

    void Update() {
        if(!up) {
            alpha -= Time.deltaTime;
            if(alpha <= 0.5f) {
                alpha = 0.5f;
                up = true;
            }
        }else {
            alpha += Time.deltaTime;
            if(alpha >= 1) {
                alpha = 1;
                up = false;
                counter++;
                if(counter > balls.Length - 1) {
                    counter = 0;
                }
            }
        }
        balls[counter].color = new Color(1, 1, 1, alpha);
    }
}
