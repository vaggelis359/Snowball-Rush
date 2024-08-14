using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndMenuScript : MonoBehaviour
{
    public GameObject gameOverUI;           // call the UI 
    public float delayBeforeRestart = 5f;   // a delay before restart

    public void GameOver()
    {
        gameOverUI.SetActive(true);         // set active Game Over UI
    }

    void Start()
    {
        gameOverUI.SetActive(true);         // Hide Game Over UI 
    }

    public void Restart()                   // Restart Game
    {
        StartCoroutine(RestartSceneAfterDelay(delayBeforeRestart));
    }
    private IEnumerator RestartSceneAfterDelay(float delay)
    {
        Physics.gravity = new Vector3(0, -9.8f, 0);
        yield return new WaitForSeconds(delay);                             // Wait delay
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   // Restart the scene
    }
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
