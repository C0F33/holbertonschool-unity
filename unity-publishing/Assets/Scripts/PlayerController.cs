using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for the Text component
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject WinLoseBG;
    public Text WinLoseText;
    public Text healthText; // Link the healthText to UI element in the Inspector
    public Text scoreText; // Link the scoreText UI element in the Inspector
    public float Speed = 5f; // Movement speed
    public int health = 5; // Player's health

    private int score; // Player's score
    private Rigidbody rb; // Rigidbody component reference

    // IEnumerator for scene reloading after a delay
    IEnumerator LoadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds); // Wait for the specified time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    void Start()
    {
        score = 0; // Initialize score
        SetScoreText(); // Update the scoreText UI
        health = 5; // Reset health if needed
        SetHealthText(); // Update the healthText UI
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        
        // Ensure WinLoseBG is inactive at the start
        WinLoseBG.SetActive(false); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            WinLoseText.text = "You Win!";
            WinLoseText.color = Color.black;
            WinLoseBG.GetComponent<Image>().color = Color.green;
            WinLoseBG.SetActive(true); // Show the WinLoseBG when player wins
            StartCoroutine(LoadScene(3)); // Reload the scene after 3 seconds
        }

        if (other.CompareTag("Trap"))
        {
            health--; // Decrease health
            SetHealthText(); // Update health UI
        }

        if (other.CompareTag("Pickup"))
        {
            score++; // Increment score
            SetScoreText(); // Update score UI
            other.gameObject.SetActive(false); // Disable the Pickup object
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            WinLoseText.text = "Game Over!";
            WinLoseText.color = Color.white;
            WinLoseBG.GetComponent<Image>().color = Color.red;
            WinLoseBG.SetActive(true); // Show the WinLoseBG when player loses
            StartCoroutine(LoadScene(3)); // Reload the scene after 3 seconds
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("menu"); // Replace "menu" with the actual name of your menu scene
        }
    }

    void FixedUpdate()
    {
        // Get input from WASD/Arrow keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a movement vector
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        // Adjust movement speed for sprinting
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            rb.linearVelocity = movement * Speed * 2; // Double speed
        }
        else
        {
            rb.linearVelocity = movement * Speed; // Normal speed
        }
    }

    void SetScoreText()
    {
        // Update the scoreText UI with the current score
        scoreText.text = "Score: " + score.ToString();
    }

    void SetHealthText()
    {
        // Update the healthText UI with the current health
        healthText.text = "Health: " + health.ToString();
    }
}
