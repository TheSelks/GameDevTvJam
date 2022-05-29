using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int correctMirrors = 0;
    public MirrorDoor mirrorDoor;
    public Mirror firstMirror;

    private void Start()
    {
        firstMirror.activateMirror();
    }
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
        if (correctMirrors == 5)
        {
            mirrorDoor.open();
        }
        else if (correctMirrors < 5)
        {
            mirrorDoor.closed();
        }
    }
}
