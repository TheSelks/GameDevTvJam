using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class CanopicJars : MonoBehaviour
{
    [SerializeField] private Image collectedJar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collectedJar.gameObject.SetActive(true);
            Debug.Log("CanopicCollected");
            Destroy(gameObject);
        }
    }
}