using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SelectedBallAnimation : MonoBehaviour {

    private bool up = false;
    private float scale = 1;

    [SerializeField]
    private GameObject _clickEffect;

  

    void OnEnable() {
        up = false;
        scale = 1;
        Taptic.Medium();
        Debug.Log("one");

    }

    void Update() {

        if(up) {

            scale += Time.deltaTime / 3;
            if(scale >= 1) {
                up = false;

               
            }
        }

        else {

            scale -= Time.deltaTime / 3;
            if(scale <= 0.8f) {
                up = true;
             
            }
        }

        if (_clickEffect == null)
            return;

        _clickEffect.SetActive(true);

        disableParticle();

        transform.localScale = new Vector2(scale, scale);

       




    }

    async void disableParticle()
    {
        await Task.Delay(1000);

        if (_clickEffect == null)
            return;

        
        _clickEffect.SetActive(false);

        
    }
    
}
