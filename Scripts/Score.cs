using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; } // Singleton instance

    public TextMeshProUGUI scoreText;
    private float score;
    private float multiplier = 15f;

    private bool hasDoubledMultiplier = false;
    private bool hasQuadrupledMultiplier = false;

    public Transform skiiMan;               // camera follow skiiMan

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
        if (scoreText == null)
        {
            //Debug.LogError("Δεν έχει οριστεί το Text component.");
            return;
        }
        score = 0;
        UpdateScoreText();
    }

    void Update()
    {
        if (skiiMan == null)
        {
            return;
        }
        score += Time.deltaTime * multiplier;

        if (score > 1000 && !hasDoubledMultiplier)
        {
            multiplier *= 4;
            hasDoubledMultiplier = true;
        }
        if (score > 5000 && !hasQuadrupledMultiplier)
        {
            multiplier *= 4;
            hasQuadrupledMultiplier = true;
        }

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }
    public void SetScore(float newScore)            // Public method to take score from another script
    {
        score = newScore;
        UpdateScoreText();
    }
    public float GetScore()                         // Publuc method to take the current score     
    {
        return score;
    }
}
