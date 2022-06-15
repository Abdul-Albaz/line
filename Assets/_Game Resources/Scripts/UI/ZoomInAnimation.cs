using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInAnimation : MonoBehaviour {

    public RectTransform rectTransform;
    private float scale = 0;
    
    void OnEnable() {
        scale = 0;
    }

    void Update() {
        scale += Time.deltaTime * 3;
        if(scale >= 1) {
            scale = 1;
            rectTransform.localScale = new Vector2(scale, scale);
            GetComponent<ZoomInAnimation> ().enabled = false;
        }
        rectTransform.localScale = new Vector2(scale, scale);
    }
}
