using Unity.VisualScripting;
using UnityEngine;

public class Player_movements : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float gravity = -20f; // Lower value for slower fall
    public float jumpHeight = 3f; // Higher value for a higher jump
    public float airControl = 0.2f; // Air control factor (0 for no control, 1 for full control)

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is grounded (on the floor)
        isGrounded = characterController.isGrounded;

        // Apply gravity when not grounded
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0f)
        {
            velocity.y = -2f; // Apply a small negative value to ensure a smooth landing
        }

        // Get input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate the movement direction
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        moveDirection = moveDirection.normalized;

        // Apply movement speed
        Vector3 horizontalMovement = moveDirection * movementSpeed;

        // Apply air control if not grounded
        if (!isGrounded && airControl > 0f)
        {
            Vector3 targetVelocity = (transform.TransformDirection(moveDirection) * movementSpeed);
            horizontalMovement = Vector3.Lerp(horizontalMovement, targetVelocity, airControl);
        }

        // Combine horizontal movement with vertical velocity
        Vector3 movement = horizontalMovement + velocity;

        // Move the character using CharacterController
        characterController.Move(movement * Time.deltaTime);

        // Jump if the player presses the jump key (e.g., Space)
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void Jump()
    {
        // Calculate the upward velocity required for the desired jump height
        float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Apply the vertical velocity to make the player jump
        velocity.y = jumpVelocity;
    }
}