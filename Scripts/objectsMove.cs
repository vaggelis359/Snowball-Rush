using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class objectsMove : MonoBehaviour
{   
    private TextMeshProUGUI scoreText;      // UI Elements
    public float speed;         //speed of objects                 


    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.FindWithTag("Score").GetComponent<TextMeshProUGUI>();    // Βρες το TextMeshProUGUI αντικείμενο με το tag "Score"

        //   if (scoreText == null)
        //   {
        //       Debug.LogError("Score Text (TMP) object not found!");
        //   }

    }

    // Update is called once per frame
    void Update()
    {
        float currentScore = GetScoreFromText();
        if (scoreText != null) { speed = 0; }
        if (currentScore > 0 ) { speed = 50; }
        if (currentScore > 1000 ) { speed = 100; }
        if (currentScore > 5000 ) { speed = 125; }
        transform.Translate(Vector3.back * Time.deltaTime * speed);

        if (transform.position.z < 75) { Destroy(gameObject); }
    }

    int GetScoreFromText()
    {
        if (scoreText != null)
        {
            string scoreString = scoreText.text.Replace("Score: ", "");
            if (int.TryParse(scoreString, out int score))
            {
                return score;
            }
        }
        return 0;
    }
}
