using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class CanopicJars : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("CanopicCollected");
            Destroy(gameObject);
        }
    }
}