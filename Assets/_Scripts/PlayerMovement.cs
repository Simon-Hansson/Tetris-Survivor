using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Input
    private Default Default;

    // Movement
    private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float groundCheckRayLength = 2f;
    private Vector2 input = Vector2.zero;
    private Vector3 forceDirection = Vector3.zero;
    private bool isMoving = false;

    // Scripts
    [SerializeField] private Camera playerCamera;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Default = new Default();
        Default.Enable();
        Default.Player.Movement.performed += MovementPerformed;
        Default.Player.Movement.canceled += MovementCanceled;
    }

    private void MovementPerformed(InputAction.CallbackContext context)
    {
        isMoving = true;
        input = context.ReadValue<Vector2>();
    }
    private void MovementCanceled(InputAction.CallbackContext context)
    {
        isMoving = false;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundCheckRayLength, Color.red);
    }
    private void FixedUpdate()
    {
        if(isMoving)
        {
            forceDirection = new(-input.x, 0, -input.y);
        }
        else
        {
            forceDirection = Vector3.zero;
        }
        rb.AddForce(forceDirection * movementForce, ForceMode.Impulse);
            
        if (Physics.gravity.y != -9.81f)
        {
            Physics.gravity = new(0, -9.81f, 0);
        }
    }
    public bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _, groundCheckRayLength))
            return true;
        else return false;
    }
}