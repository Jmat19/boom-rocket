using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBody : MonoBehaviour
{
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private float xRot;

    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Rigidbody rb;
    [Space]
    [SerializeField] private float Speed;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float Jumpforce;

    // Update is called once per frame
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse x"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MovePlayerCamera();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * Speed;
        rb.velocity = new Vector3(MoveVector.x, rb.velocity.y, MoveVector.z);

        if(Input.GetKeyDown (KeyCode.Space))
        {
            rb.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
        }
    }

    private void MovePlayerCamera()
    {
        xRot -= PlayerMouseInput.y * Sensitivity;

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f); 
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f , 0f);
    }
}
