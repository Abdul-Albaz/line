 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public Sprite tile1;
    public Sprite tile2;
    public Sprite tile3;

    void Start() {
        if(PlayerPrefs.GetInt("TileType") == 0) {
            GetComponent<SpriteRenderer> ().sprite = tile1;
        }else if(PlayerPrefs.GetInt("TileType") == 1) {
            GetComponent<SpriteRenderer> ().sprite = tile2;
        }else if(PlayerPrefs.GetInt("TileType") == 2) {
            GetComponent<SpriteRenderer> ().sprite = tile3;
        }
    }

    void OnMouseDown() {
        if(Vars.isBallMoving) return;

        string name = this.gameObject.name;

        int from = name.IndexOf("e") + "e".Length;
        int to = name.LastIndexOf("X");
        string x = name.Substring(from, to - from);

        from = name.IndexOf("X") + "X".Length;
        to = name.Length;
        string y = name.Substring(from, to - from);


        if(transform.Find("Ball") != null) {
            if(Vars.ball != null) {
                Vars.ball.GetComponent<SelectedBallAnimation> ().enabled = false;
                Vars.ball.transform.localScale = new Vector2(1, 1);
            }
            Vars.ballStartPosX = Int32.Parse(x);
            Vars.ballStartPosY = Int32.Parse(y);
            Vars.ball = transform.Find("Ball").gameObject;
            Vars.ball.GetComponent<SelectedBallAnimation> ().enabled = true;
            GameObject.Find("BallSelectSound").GetComponent<AudioSource> ().Play();
        }else {
            if(Vars.ballStartPosX == -1) return;
            int xPos = Int32.Parse(x);
            int yPos = Int32.Parse(y);
            GameObject.Find("GameManager").GetComponent<BallsPathfinder> ().InitializeBallMovement(Vars.ball, xPos, yPos);
        } 
    }
}
