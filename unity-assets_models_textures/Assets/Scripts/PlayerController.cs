using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    private Vector3 velocity;
    private bool isGrounded;
    public Vector3 startPosition;
    private Rigidbody rb; // Add reference for Rigidbody

    void Start()
    {
        controller = GetComponent<CharacterController>();
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>(); // Initialize Rigidbody
        rb.isKinematic = true; // Set Rigidbody to Kinematic

        Debug.Log("Start Position: " + startPosition); // Log start position when the game begins
    }

    void Update()
    {
        // Check if player is grounded
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }

        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Get camera's forward and right direction (ignoring Y-axis)
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Determine movement direction relative to the camera
        Vector3 moveDirection = camForward * moveZ + camRight * moveX;
        moveDirection.Normalize();

        // Move the character
        if (moveDirection.magnitude > 0)
        {
            controller.Move(moveDirection * speed * Time.deltaTime);

            // Rotate player to match movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Handle Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Apply jump force
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OutofBounds"))
        {
            Debug.Log("Player has fallen out of bounds!");
            Respawn();
        }
    }

    void Respawn()
    {
        Debug.Log("Respawn called!");
        controller.enabled = false; // Disable the controller to avoid conflicts
        transform.position = startPosition + Vector3.up * 5f; // Respawn slightly above the start position
        controller.enabled = true; // Re-enable the controller after position is updated
        velocity = Vector3.zero;
        Debug.Log("Player respawned at: " + transform.position);
    }


    // Log position when the game stops
    private void OnApplicationQuit()
    {
        Debug.Log("End Position (when game stops): " + transform.position); // Log the position when the game is stopped
    }
}
