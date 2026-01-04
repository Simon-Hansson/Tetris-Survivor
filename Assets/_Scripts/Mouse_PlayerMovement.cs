using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Mouse_PlayerMovement : MonoBehaviour
{
    // Input
    private Default _default;
    private InputAction _mousePositionAction;
    private InputAction _openInventoryAction;

    private Rigidbody _rb;
    private bool _isUI = false;

    [Header("Inspector References")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private GameObject _UI_Inventory;

    [Header("Movement")]
    [SerializeField] private float _movementForce = 8f;
    private Vector2 _input = Vector2.zero;
    private Vector3 _forceDirection = Vector3.zero;
    private bool _isMoving = false;
    //[SerializeField] private float groundCheckRayLength = 2f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _default = new Default();
        _default.Enable();
        _default.Player.MouseClick.performed += MouseClickPerformed;
        _default.Player.MouseClick.canceled += MouseClickCanceled;
        _mousePositionAction = _default.Player.MousePosition;
        _openInventoryAction = _default.Player.Keyboard;
    }

    private void MouseClickPerformed(InputAction.CallbackContext context)
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = _mousePositionAction.ReadValue<Vector2>();
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        if (results.Where(r => r.gameObject.layer == 5).Count() > 0) //6 being my UILayer
        {
            Debug.Log(results[0].gameObject.name); //The UI Element
            _isUI = true;
        }
        else
        {
            _isUI = false;
            _isMoving = true;
        }
    }
    private void MouseClickCanceled(InputAction.CallbackContext context)
    {
        _isMoving = false;

    }

    private void Update()
    {
        //Debug.DrawRay(transform.position, Vector3.down * groundCheckRayLength, Color.red);
        if (_openInventoryAction.triggered)
        {
            _UI_Inventory.SetActive(!_UI_Inventory.activeSelf);
        }
    }
    private void FixedUpdate()
    {

        Vector2 mousePosition = _mousePositionAction.ReadValue<Vector2>();
        Ray ray = _playerCamera.ScreenPointToRay(mousePosition);
        if (_isMoving && !_isUI && Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Debug.Log("Hit: " + hitInfo.collider.name);
            Vector3 targetPosition = hitInfo.point;
            Vector3 direction = (targetPosition - transform.position).normalized;
            _input = new Vector2(direction.x, direction.z);
            _forceDirection = new(_input.x, 0, _input.y);
        }
        else
        {
            _forceDirection = Vector3.zero;
        }
        _rb.AddForce(_forceDirection * _movementForce, ForceMode.Impulse);

        if (_isUI)
        {
            
        }

        if (Physics.gravity.y != -9.81f)
        {
            Physics.gravity = new(0, -9.81f, 0);
        }
    }
    //public bool IsGrounded()
    //{
    //    if (Physics.Raycast(transform.position, Vector3.down, out _, groundCheckRayLength))
    //        return true;
    //    else return false;
    //}
}