using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureDoor : MonoBehaviour
{
    //public Transform startMarker;
    //public Transform endMarker;
    //public float speed = 1f;
    //private float startTime;
    //private float journeyLength;
    private Animator _pressureDoorAnim;

    private void Start()
    {
        //startTime = Time.time;
        //journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        _pressureDoorAnim = GetComponent<Animator>();
    }

    public void Open()
    {
        //float distCovered = (Time.time - startTime) * speed;
        //float fractionOfJourney = distCovered / journeyLength;
        //transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
        _pressureDoorAnim.SetTrigger("OpenDoor");
    }

    public void Closed()
    {
        //float distCovered = (Time.time - startTime) * speed;
        //float fractionOfJourney = distCovered / journeyLength;
        //transform.position = Vector3.Lerp(endMarker.position, startMarker.position, fractionOfJourney);
        _pressureDoorAnim.enabled = true;
    }

    public void AnimationPause()
    {
        _pressureDoorAnim.enabled = false;
    }

}
