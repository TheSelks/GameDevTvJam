using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public float[] correctRotation;
    public GameController gameController;
    public bool correct = false;

    private void Awake()
    {
        rotationCheck();
    }
    private void positivePlacementResult()
    {
        correct = true;
        gameController.mirrorCorrect();
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
        transform.Rotate(new Vector3(0, 45, 0));
        rotationCheck();
    }
}
