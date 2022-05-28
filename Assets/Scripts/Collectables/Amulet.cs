using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Amulet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Healed");
            other.gameObject.GetComponent<ThirdPersonController>().Heal();
            Destroy(gameObject);
        }
    }
}
