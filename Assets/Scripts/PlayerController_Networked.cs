using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController_Networked : NetworkBehaviour
{
    public float moveSpeed = 20.0f;

    private Rigidbody _rb;
    private Vector2 _moveInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!IsLocalPlayer) return;
        
        Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);

        _rb.AddForce(moveDirection * moveSpeed, ForceMode.Force);
    }
}