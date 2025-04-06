using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 8f;
    public float gravity = -20f;

    [Header("References")]
    public Transform cameraTransform;
    public Transform playerModel;
    public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector3 currentGravityDirection = Vector3.down;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleFallAnimation();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        Vector3 moveDir = (camRight * input.x + camForward * input.z).normalized;

        if (moveDir.magnitude > 0.1f)
        {
            controller.Move(moveDir * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, targetRotation, 10f * Time.deltaTime);

            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    void HandleJump()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            // Reset jump/fall animations when landed
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            PlayJumpAnimation(); // Play jump at same time as movement
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(Vector3.up * velocity.y * Time.deltaTime);
    }

    void HandleFallAnimation()
    {
        if (!isGrounded && velocity.y < 0)
        {
            animator.SetBool("IsFalling", true);
        }
        else
        {
            animator.SetBool("IsFalling", false);
        }
    }

    void PlayJumpAnimation()
    {
        animator.SetTrigger("Jump"); // Use Trigger for single-frame transition
        animator.SetBool("IsJumping", true); // Optional: for blending or transition checks
    }

    public void ChangeGravity(Vector3 newGravity)
    {
        currentGravityDirection = newGravity.normalized;
        Physics.gravity = currentGravityDirection * Mathf.Abs(gravity);
    }
}
