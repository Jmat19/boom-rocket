using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Boost : MonoBehaviour
{
    [Header("Reference")]
    public Transform orientation;
    public Transform playerCamera;
    private CharacterController charCon;
    private PlayerController pc;

    [Header("Dashing")]
    public float boostForce;
    public float boostUpwardForce;
    public float boostDuration;

    [Header("Cooldown")]
    public float boostCd;
    private float boostCdTimer;

    private void Start()
    {
        charCon = GetComponent<CharacterController>();
        pc = GetComponent<PlayerController>();
    }

    private void Update()
    {
        
    }

    private void Dash(InputAction.CallbackContext context)
    {
        Vector3 forceToApply = orientation.forward * boostForce + orientation.up * boostUpwardForce;

        charCon.SimpleMove(forceToApply * boostForce);

        Invoke(nameof(ResetBoost), boostDuration);
    }

    private void ResetBoost()
    {

    }
}
