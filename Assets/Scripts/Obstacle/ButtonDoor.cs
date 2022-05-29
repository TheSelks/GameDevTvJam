using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
    //public Transform startMarker;
    //public Transform endMarker;
    //public float speed = 1f;
    //private float startTime;
    //private float journeyLength;
    private Animator _buttonDoorAnim;

    private void Start()
    {
        _buttonDoorAnim = GetComponent<Animator>();
        //startTime = Time.time;
        //journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    public void open()
    {
        //float distCovered = (Time.time - startTime) * speed;
        //float fractionOfJourney = distCovered / journeyLength;
        _buttonDoorAnim.SetTrigger("OpenDoor");
        //transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
    }

    public void closed()
    {
        //float distCovered = (Time.time - startTime) * speed;
        //float fractionOfJourney = distCovered / journeyLength;
        _buttonDoorAnim.enabled = true;
        //transform.position = Vector3.Lerp(endMarker.position, startMarker.position, fractionOfJourney);
    }

    public void AnimationPause()
    {
        _buttonDoorAnim.enabled = false;
    }
}
