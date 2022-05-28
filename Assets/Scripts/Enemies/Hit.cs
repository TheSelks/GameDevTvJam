using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Hit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit by Bat");
            // False result removes health
            other.gameObject.GetComponent<ThirdPersonController>().Hit();
        }
    }
}
