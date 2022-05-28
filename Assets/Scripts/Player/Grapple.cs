using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] private float pullSpeed = 0.5f;
    [SerializeField] private float stopDistance = 4f;
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private Transform shootTransform;
    private StarterAssetsInputs _input;

    private Hook hook;
    private bool pulling;
    private Rigidbody rigid;


    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        rigid = GetComponent<Rigidbody>();
        pulling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hook == null && _input.fire)
        {
            StopAllCoroutines();
            pulling = false;
            hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
            hook.Initialize(this, shootTransform);
            StartCoroutine(DestroyHookAfterLifetime());
        }

        // if()
    }

    public void StartPull()
    {
        pulling = true;
    }

    private void DestroyHook()
    {
        if (hook == null) return;

        pulling = false;
        Destroy(hook.gameObject);
        hook = null;
    }

    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(8f);

        DestroyHook();
    }
}
