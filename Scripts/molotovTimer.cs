using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class molotovTimer : MonoBehaviour
{
    public static molotovTimer Instance { get; private set; } // Singleton instance

    public TextMeshProUGUI scoreText;
    public Image molotovImage;
    public GameObject skiiMan;

    private float molotovTimeLeft;
    private bool isMolotovActive;
    private PlayerMoves playerMoves;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        molotovTimeLeft = 0;
        isMolotovActive = false;
        if (molotovImage != null)
        {
            molotovImage.enabled = false;
        }
        if (scoreText != null)
        {
            scoreText.enabled = false;
        }
        else
        {
            Debug.LogError("Δεν έχει οριστεί το Text component.");
        }
        playerMoves = skiiMan.GetComponent<PlayerMoves>();
        if (playerMoves == null)
        {
            Debug.LogError("Δεν βρέθηκε το PlayerMoves component στο skiiMan.");
        }
    }

    void Update()
    {
        if (playerMoves.HasTakenMolotov() && !isMolotovActive)          // Check if the player has taken a Molotov
        {
            isMolotovActive = true;
            molotovTimeLeft = 10;                                       // Start the countdown from 10 seconds


            UiOn();                                                     // Show the UI elements
        }
        if (isMolotovActive)
        {
            molotovTimeLeft -= Time.deltaTime;                          // Decrease the timer
            UpdateScoreText();

            if (molotovTimeLeft <= 0 || Input.GetKeyDown(KeyCode.E))    // Check if the timer has reached zero or if the player presses the "E" key
            {

                UiOff();                                                // Hide the UI elements when the time is up or "E" is pressed
                isMolotovActive = false;

                playerMoves.SetHasTakenMolotov(false);                  // Update PlayerMoves he use molotov
            }
        }
    }
    private void UiOn()
    {
        molotovImage.enabled = true;
        scoreText.enabled = true;
    }
    private void UiOff()
    {
        molotovImage.enabled = false;
        scoreText.enabled = false;
    }
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = Mathf.FloorToInt(molotovTimeLeft).ToString();
        }
    }
    public void SetScore(float newScore)                            // Public method to take score from another script
    {
        molotovTimeLeft = newScore;
        UpdateScoreText();
    }

    public float GetScore()                                         // Publuc method to take the current score 
    {
        return molotovTimeLeft;
    }
}
