using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class CanopicJars : MonoBehaviour
{
   // [SerializeField] private Image thisJar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           // thisJar.enabled = true;
            Debug.Log("CanopicCollected");
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}