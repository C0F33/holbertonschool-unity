using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  // Assign the player in the Inspector
    public float distance = 5f;  // Distance from player
    public float sensitivity = 3f;  // Mouse sensitivity
    public float orbitSpeed = 100f; // Speed for Q and E rotation
    public float yMin = -20f, yMax = 80f;  // Limit vertical rotation

    private float yaw = 0f;  // Horizontal rotation
    private float pitch = 0f; // Vertical rotation

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        // Mouse rotation (Right-click + drag)
        if (Input.GetMouseButton(1)) // Right-click to rotate camera
        {
            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        }

        // Keyboard orbit (Q and E)
        if (Input.GetKey(KeyCode.Q)) yaw -= orbitSpeed * Time.deltaTime; // Rotate left
        if (Input.GetKey(KeyCode.E)) yaw += orbitSpeed * Time.deltaTime; // Rotate right

        // Clamp vertical rotation
        pitch = Mathf.Clamp(pitch, yMin, yMax);

        // Calculate new camera position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = player.position + offset;
        transform.LookAt(player.position); // Keep camera looking at player
        
        // Smoothly move the camera to prevent snapping after respawn
        transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * 5f);
    
        transform.LookAt(player.position);
    }
}