using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public PressureDoor pressureDoor;
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "Pressure Plate 1":
                pressureDoor.Open();
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "Pressure Plate 1":
                pressureDoor.Closed();
                break;
        }
    }
}
