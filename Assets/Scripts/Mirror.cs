using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public float[] correctRotation;
    int positiveRotations = 1;
    public GameController gameController;
    public bool correct = false;

    private void Awake()
    {
        positiveRotations = correctRotation.Length;
        rotationCheck();
    }
    private void positivePlacementResult()
    {
        correct = true;
        gameController.mirrorCorrect();
    }
    private void positivePlacementResultChanged(bool correct)
    {
        correct = false;
        gameController.mirrorWrong();
    }
    private void rotationCheck()
    {
        if (positiveRotations > 1)
        {
            if ((int)transform.eulerAngles.y == correctRotation[0] && correct == false)
            {
                positivePlacementResult();
            }
            else if (correct == true)
            {
                positivePlacementResultChanged(correct);
            }
        }
    }
    public void rotate()
    {
        Debug.Log("work");
        transform.Rotate(new Vector3(0, 45, 0));
        rotationCheck();
    }
}
