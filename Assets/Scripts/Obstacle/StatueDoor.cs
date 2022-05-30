using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueDoor : MonoBehaviour
{
    private Animator _statueDoorAnim;
 
    private void Start()
    {
        _statueDoorAnim = GetComponent<Animator>();
    }

    public void open()
    {
        _statueDoorAnim.SetTrigger("OpenDoor");
    }

    public void closed()
    {
        _statueDoorAnim.enabled = true;
    }

    public void AnimationPause()
    {
        _statueDoorAnim.enabled = false;
    }
}
