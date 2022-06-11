using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedBallAnimation : MonoBehaviour {
    private bool up = false;
    private float scale = 1;
    
    void OnEnable() {
        up = false;
        scale = 1;
    }

    void Update() {
        if(up) {
            scale += Time.deltaTime / 3;
            if(scale >= 1) {
                up = false;
            }
        }else {
            scale -= Time.deltaTime / 3;
            if(scale <= 0.8f) {
                up = true;
            }
        }
        transform.localScale = new Vector2(scale, scale);
    }
}
