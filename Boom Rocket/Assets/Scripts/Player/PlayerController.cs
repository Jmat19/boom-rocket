using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;

    [SerializeField] private float speed;

    [SerializeField] private float jumpStrength;
    private int _numberOfJumps;
    [SerializeField] private int maxNumberOfJumps = 2;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    //Updates everytime the player moves and turns in a direction | Use of gravity on player
    private void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }
       
        _direction.y = _velocity;
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void ApplyMovement()
    {
        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    //calls from the input system to allow the player to move with WASD
    public void Move (InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps) return;
        if (_numberOfJumps == 0) StartCoroutine(WaitForLanding());

        // _numberOfJumps = _numberOfJumps + 1; {Alternate Code}
        // _numberOfJumps += 1; {Alternate Code}
        _numberOfJumps++;

        //Used for equal jump strength
        _velocity = jumpStrength;

        //Used for slightly lower Double Jump
        //_velocity = jumpStrength / _numberOfJumps;
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());

        // while (!IsGrounded()) yield return null {Alternate Code}
        yield return new WaitUntil(IsGrounded);

        _numberOfJumps = 0;
    }

    public bool IsGrounded() => _characterController.isGrounded;
}
