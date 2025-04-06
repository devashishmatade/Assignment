 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCharacter : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 10f;
    public Transform orientation;
    public Animator animator;

    private Rigidbody rb;
    private Vector3 gravityDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        GravityManager.Instance.GravityChanged += OnGravityChanged;

        OnGravityChanged(GravityManager.Instance.GetGravityDirection());
    }

    void FixedUpdate()
    {
        Move();
        ApplyCustomGravity();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = (orientation.right * h + orientation.forward * v).normalized;
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    void ApplyCustomGravity()
    {
        rb.AddForce(gravityDirection * GravityManager.Instance.gravityStrength, ForceMode.Acceleration);
    }

    void OnGravityChanged(Vector3 newDirection)
    {
        gravityDirection = newDirection;
        AlignToGravity();
    }

    void AlignToGravity()
    {
        // Align player's up direction smoothly
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        // Align orientation transform (used for movement direction)
        if (orientation != null)
        {
            Vector3 newForward = Vector3.Cross(orientation.right, -gravityDirection);
            orientation.rotation = Quaternion.LookRotation(newForward, -gravityDirection);
        }
    }


    public void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(-gravityDirection * jumpForce, ForceMode.Impulse);
            animator?.SetTrigger("Jump");
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -gravityDirection, 1.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            Vector3 surfaceNormal = collision.contacts[0].normal;
            GravityManager.Instance.SetGravityFromSurfaceNormal(surfaceNormal);
        }
    }
}