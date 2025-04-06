using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PointCollector : MonoBehaviour
{
    public int score = 0;
    public Text scoreText; // Assign in Inspector

    private void Start()
    {
        UpdateScoreUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            score += 1;
            UpdateScoreUI();

            // Optional: play sound or VFX here
            Destroy(other.gameObject);
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Points: " + score;
        }
    }
}