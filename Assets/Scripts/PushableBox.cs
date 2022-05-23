using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public PressureDoor pressureDoor;
    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.name)
        {
            case "Pressure Plate 1":
                pressureDoor.open();
                break;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        switch (other.gameObject.name)
        {
            case "Pressure Plate 1":
                pressureDoor.closed();
                break;
        }
    }
}
