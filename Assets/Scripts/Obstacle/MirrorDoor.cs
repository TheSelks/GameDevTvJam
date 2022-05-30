using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorDoor : MonoBehaviour
{
    private Animator _mirrorDoorAnim;

    private void Start()
    {
        _mirrorDoorAnim = GetComponent<Animator>();
    }

    public void Open()
    {
        _mirrorDoorAnim.SetTrigger("OpenDoor");
    }

    public void Closed()
    {
        _mirrorDoorAnim.enabled = true;
    }

    public void AnimationPause()
    {
        _mirrorDoorAnim.enabled = false;
    }

}
