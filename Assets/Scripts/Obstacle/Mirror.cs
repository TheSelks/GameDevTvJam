using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public float[] correctRotation;
    public GameController gameController;
    public bool correct = false;
    public bool canMove;
    public Mirror nextMirror;
    public GameObject lightBeam;

    private void Awake()
    {
        rotationCheck();
        canMove = false;
        lightBeam.SetActive(false);
    }
    private void positivePlacementResult()
    {
        correct = true;
        canMove = false;
        gameController.mirrorCorrect();
        nextMirror.activateMirror();
        lightBeam.SetActive(true);
    }

    public void activateMirror()
    {
        canMove = true;
    }
    private void positivePlacementResultChanged()
    {
        correct = false;
        gameController.mirrorWrong();
    }
    private void rotationCheck()
    {
        if (Mathf.RoundToInt(transform.eulerAngles.y) == correctRotation[0] && correct == false)
        {
            positivePlacementResult();
        }
        else if (correct == true)
        {
            positivePlacementResultChanged();
        }
    }
    public void rotate()
    {
        Debug.Log("work");
        if (canMove == true)
        {
            transform.Rotate(new Vector3(0, 90, 0));
            rotationCheck();
        }
    }
}
