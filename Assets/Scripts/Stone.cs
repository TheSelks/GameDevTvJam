using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class Stone : MonoBehaviour
{
    [SerializeField] private int stoneNumber;
    [SerializeField] private Image collectedStone;
    [SerializeField] private ThirdPersonController thirdPersonController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collectedStone.gameObject.SetActive(true);
            Debug.Log("CanopicCollected");

            if(stoneNumber == 1)
                thirdPersonController.stone1 = true;

            if (stoneNumber == 2)
                thirdPersonController.stone2 = true;

            if (stoneNumber == 3)
                thirdPersonController.stone3 = true;

            if (stoneNumber == 4)
                thirdPersonController.stone4 = true;

            if (stoneNumber == 5)
                thirdPersonController.stone5 = true;

            if (stoneNumber == 6)
                thirdPersonController.stone6 = true;
            
            Destroy(gameObject.transform.parent.gameObject);
        }
    }


}
