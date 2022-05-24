using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class PrecisionControls : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    
    private CharacterController characterController;
    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;

    private Vector3 characterVelocity;

    private Vector3 grapplePosition;
    private float oldGravity = -15f;

    [Tooltip("This will alter the minimum speed that you travel towards grapple target.")] 
    [SerializeField] private float grappleSpeedMin = 10f;

    [Tooltip("This will alter the minimum speed that you travel towards grapple target.")] 
    [SerializeField] private float grappleSpeedMax = 40f;

    [Tooltip("This multiplier will adjust travel speed for grapple")] 
    private float grappleSpeedMultiplier = 2f;

    [SerializeField] float grappleSpeed = 0;

    private State state;

    private enum State
    {
        Normal,
        Grappling
    }
    
    Vector2 screenCentrePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();

        state = State.Normal;
    }

    private void Update()
    {
        switch (state)
        {
            default:
                case State.Normal:
                HandleGrappleStart();

                Vector3 mouseWorldPosition = Vector3.zero;

                // Since the reticile is pointed at the centre of the screen no need to find mouse position
                // Just find use that for targeting
                Ray ray = Camera.main.ScreenPointToRay(screenCentrePoint);

                if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
                {
                    mouseWorldPosition = raycastHit.point;
                }
                
                if (starterAssetsInputs.aim)
                {
                    aimCamera.gameObject.SetActive(true);
                    thirdPersonController.SetSensitivity(aimSensitivity);
                    thirdPersonController.SetRotateOnMove(false);

                    Vector3 worldAimTarget = mouseWorldPosition;
                    worldAimTarget.y = transform.position.y;
                    Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

                    transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
                }
                else
                {
                    aimCamera.gameObject.SetActive(false);
                    thirdPersonController.SetSensitivity(normalSensitivity);
                    thirdPersonController.SetRotateOnMove(true);
                }

                break;

            case State.Grappling:
                HandleGrappleMovement();
                break;

        }
    }

    private void HandleGrappleStart()
    {
        if (GrappleInput())
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(screenCentrePoint), out RaycastHit raycastHit))
            {

                // Something has been hit
                Debug.Log("Hit");
                thirdPersonController.enabled = false;

                grapplePosition = raycastHit.point;
                state = State.Grappling;

                // Set gravity to 0 so that you can actually zoom to the grapple point
                thirdPersonController.Gravity = 0f;
                thirdPersonController.Grounded = false;
                
            }
        }
    }

    private void HandleGrappleMovement()
    {
        Vector3 grappleDirection = (grapplePosition - transform.position).normalized;
        grappleSpeed = Mathf.Clamp(Vector3.Distance(transform.position, grapplePosition), grappleSpeedMin,
            grappleSpeedMax);
        

        characterController.Move(grappleDirection * grappleSpeed * grappleSpeedMultiplier * Time.deltaTime);

        float reachedDistance = 2f;
        if (Vector3.Distance(transform.position, grapplePosition) < reachedDistance)
        {
            CancelGrapple();
        }

        if (GrappleInput())
        {
            CancelGrapple();
        }
    }

    private void CancelGrapple()
    {
        state = State.Normal;
        thirdPersonController.Gravity = oldGravity;
        thirdPersonController.enabled = true;
    }

    private bool GrappleInput()
    {
        return starterAssetsInputs.fire;
    }
}
