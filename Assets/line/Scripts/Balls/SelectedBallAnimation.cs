using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SelectedBallAnimation : MonoBehaviour {

    private bool up = false;
    private float scale = 1;

    [SerializeField]
    private GameObject _clickEffict;

  

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

        if (_clickEffict == null)
            return;

        _clickEffict.SetActive(true);

        transform.localScale = new Vector2(scale, scale);

        disableParticle();




    }

    async void disableParticle()
    {
        await Task.Delay(1000);
        if (_clickEffict == null)
            return;
        _clickEffict.SetActive(false);

        
    }
    
}
