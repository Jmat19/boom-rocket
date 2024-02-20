using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float jumpSpeed;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private bool canDoubleJump;

    private Vector3 forceDirection;

    InputSystem inputActions;
    InputAction move;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem();
        isGrounded = true;
    }

    void OnEnable()
    {
        inputActions.Land.Jump.performed += OnJump;
        move = inputActions.Land.Movement;
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Land.Jump.performed -= OnJump;
        inputActions.Disable();
    }

    void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            isGrounded = false;
        }
        else if (canDoubleJump)
        {
            rb.velocity = Vector3.up * jumpSpeed;
            canDoubleJump = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        forceDirection += move.ReadValue<Vector2>().x * transform.right * movementSpeed;
        forceDirection += move.ReadValue<Vector2>().y * transform.forward * movementSpeed;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        Vector3 horizontalVel = rb.velocity;
        horizontalVel.y = 0;
        if (horizontalVel.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVel.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }
    }
}
