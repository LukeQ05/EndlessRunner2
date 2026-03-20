using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("Difficulty")]
    public float baseSpeed = 5f;
    public float speedIncreaseRate = 0.1f;   // units/sec per second survived
    public float maxSpeed = 18f;
    public float difficultyRampInterval = 5f; // spawn rate tightens every N seconds

    // Runtime state
    public float CurrentSpeed { get; private set; }
    public bool IsRunning { get; private set; }
    public float Score { get; set; }

    private float highScore;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        Score        = 0;
        CurrentSpeed = baseSpeed;
        IsRunning    = true;
        Time.timeScale = 1f;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (!IsRunning) return;

        // Accumulate score (distance-based feel)
        Score        += Time.deltaTime * CurrentSpeed;
        CurrentSpeed  = Mathf.Min(CurrentSpeed + speedIncreaseRate * Time.deltaTime, maxSpeed);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText    != null) scoreText.text    = "Score: " + Mathf.FloorToInt(Score);
        if (highScoreText!= null) highScoreText.text = "Best: "  + Mathf.FloorToInt(highScore);
    }

    public void GameOver()
    {
        IsRunning = false;

        if (Score > highScore)
        {
            highScore = Score;
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
        }

        if (gameOverPanel  != null) gameOverPanel.SetActive(true);
        if (finalScoreText != null)
            finalScoreText.text = "Score: " + Mathf.FloorToInt(Score)
                                + "\nBest: " + Mathf.FloorToInt(highScore);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Scene 0 = Main Menu
    }

    // Returns 0-1 difficulty progress (used by spawner)
    public float DifficultyFactor => Mathf.InverseLerp(baseSpeed, maxSpeed, CurrentSpeed);
}