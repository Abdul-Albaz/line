using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOutAnimation : MonoBehaviour {
    
    public RectTransform rectTransform;
    private float scale = 1;
    
    void OnEnable() {
        scale = 1;
    }

    void Update() {
        scale -= Time.deltaTime * 3;
        if(scale <= 0) {
            scale = 0;
            rectTransform.localScale = new Vector2(scale, scale);
            GetComponent<ZoomOutAnimation> ().enabled = false;
        }
        rectTransform.localScale = new Vector2(scale, scale);
    }
}
