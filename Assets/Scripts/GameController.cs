using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int correctMirrors = 0;
    public MirrorDoor mirrorDoor;

    public void mirrorCorrect()
    {
        correctMirrors += 1;
    }

    public void mirrorWrong()
    {
        correctMirrors -= 1;
    }

    private void Update()
    {
        if (correctMirrors == 4)
        {
            mirrorDoor.open();
        }
        else if (correctMirrors < 4)
        {
            mirrorDoor.closed();
        }
    }
}
