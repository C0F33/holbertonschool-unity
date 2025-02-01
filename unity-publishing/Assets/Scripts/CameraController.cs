using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    private Vector3 offset;   // Offset between the camera and player

    void Start()
    {
        // Calculate and store the offset value by subtracting the player's position from the camera's position
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        // Set the camera's position to the player's position plus the offset
        transform.position = player.transform.position + offset;
    }
}