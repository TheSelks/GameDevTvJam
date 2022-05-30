using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int correctMirrors = 0;
    public int activatedStatues = 0;
    public MirrorDoor mirrorDoor;
    public Mirror firstMirror;
    public StatueDoor statueDoor;

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
    public void statueActivated()
    {
        activatedStatues += 1;
    }

    private void Update()
    {
        if (correctMirrors == 5)
        {
            mirrorDoor.Open();
        }
        else if (correctMirrors < 5)
        {
            mirrorDoor.Closed();
        }

        if (activatedStatues == 6)
        {
            statueDoor.open();
        }
    }
}
