using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<CharacterMovement>();
    }

    void Update()
    {
        HandleMovementAnimations();
        HandleJumpAnimation();
    }

    private void HandleMovementAnimations()
    {
        float speed = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude;
        animator.SetFloat("Speed", speed);
    }

    private void HandleJumpAnimation()
    {
     //   animator.SetBool("IsJumping", !playerMovement.IsGrounded());
    }
}
