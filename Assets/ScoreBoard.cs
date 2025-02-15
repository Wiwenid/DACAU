using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public static Scoreboard Instance { get; private set; }
    
    private int score = 0;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        UpdateScoreText();
    }

    public static void AddPoints(int points)
    {
        if (Instance != null)
        {
            Instance.score += points;
            Instance.UpdateScoreText();
        }
    }
    public static void ResetScore()
    {
        if (Instance != null)
        {
            Instance.score = 0;
            Instance.UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}