using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float timeLimit = 120f;
    public Text timerText;

    private float timer;
    private bool isGameOver = false;

    void Start()
    {
        timer = timeLimit;
    }

    void Update()
    {
        if (isGameOver) return;
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        timer -= Time.deltaTime;
        timerText.text = Mathf.Ceil(timer).ToString();

        if (timer <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CollectCube()
    {
        if (GameObject.FindGameObjectsWithTag("Collectible").Length == 0)
        {
            Debug.Log("All Cubes Collected!");
            // Add victory logic here
        }
    }
}
