using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _grapplingHook;
    [SerializeField] private Transform _handPos;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private LayerMask _grappleLayer;
    [SerializeField] private float _maxGrappleDistance;
    [SerializeField] private float _hookSpeed;
    [SerializeField] private Vector3 _offset;

    private PlayerInput _playerInput;

    private bool isShooting, isGrappling;

    Vector2 screenCentrePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
    private Vector3 _hookPoint;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        isShooting = false;
        isGrappling = false;
        _lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (_grapplingHook.parent == _handPos)
        {
            _grapplingHook.localPosition = Vector3.zero;
            _grapplingHook.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (_playerInput.actions["Fire"].WasPressedThisFrame())
        {
            ShootHook();
        }

        if (isGrappling)
        {
            _grapplingHook.position = Vector3.Lerp(_grapplingHook.position, _hookPoint, _hookSpeed * Time.deltaTime);
            if (Vector3.Distance(_grapplingHook.position, _hookPoint) < 0.5f)
            {
                _controller.enabled = false;
                _playerBody.position = Vector3.Lerp(_playerBody.position, _hookPoint - _offset, _hookSpeed * Time.deltaTime);
                if (Vector3.Distance(_playerBody.position, _hookPoint-_offset) < 0.5f)
                {
                    _controller.enabled = true;
                    isGrappling = false;
                    _grapplingHook.SetParent(_handPos);
                    _lineRenderer.enabled = false;
                }
            }

        }
    }

    private void LateUpdate()
    {
        _lineRenderer.SetPosition(0, _grapplingHook.position);
        _lineRenderer.SetPosition(1, _handPos.position);
    }

    private void ShootHook()
    {
        if (isShooting || isGrappling) return;

        isShooting = true;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenCentrePoint);

        if (Physics.Raycast(ray, out hit, _maxGrappleDistance, _grappleLayer))
        {
            _playerBody.transform.LookAt(hit.point);
            _hookPoint = hit.point;
            isGrappling = true;
            _grapplingHook.parent = null;
            _grapplingHook.LookAt(_hookPoint);
            _lineRenderer.enabled = true;

            print("HIT!");
        }

        isShooting = false;
    }
}
