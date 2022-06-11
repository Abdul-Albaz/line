using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vars : MonoBehaviour {
    public static int[,] fields;
    public static int ballStartPosX = -1;
    public static int ballStartPosY = -1;
    public static GameObject ball;
    public static bool isBallMoving = false;
    public static int score = 0;
    public static int currentMode = 0;//0 - 7x7, 1 - 9x9, 2 - 11x11

    public static void ResetAllVariables() {
        ballStartPosX = -1;
        ballStartPosY = -1;
        isBallMoving = false;
        score = 0;
    }
}
