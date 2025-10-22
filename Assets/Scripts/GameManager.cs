using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject gameOverPanel;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI knivesLeftText;
    public TextMeshProUGUI levelText;

    [Header("Game Over UI")]
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI finalScoreText;

    [Header("Level Settings")]
    public int level = 1;
    public int score = 0;
    private int knivesLeft;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Show start panel first
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    // --- Called by Play Button ---
    public void StartGame()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);

        score = 0;
        level = 1;
        gameEnded = false;

        StartLevel(level);
    }

    // --- Knife & Score Methods ---
    public void AddScore(int points)
    {
        if (gameEnded) return;
        score += points;
        UpdateUI();
    }

    public void UseKnife()
    {
        if (gameEnded) return;

        knivesLeft--;
        UpdateUI();

        if (knivesLeft <= 0)
        {
            WinLevel();
        }
    }

    public void KnifeHitKnife()
    {
        LoseGame();
    }

    // --- Win/Lose Methods ---
    void WinLevel()
    {
        gameEnded = true;

        if (gameOverText != null) gameOverText.text = "You Win!";
        if (finalScoreText != null) finalScoreText.text = "Score: " + score;

        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        // BREAK TRUNK
        TrunkRotate trunk = FindObjectOfType<TrunkRotate>();
        if (trunk != null)
            trunk.BreakTrunk();

        Invoke(nameof(NextLevel), 2f);
    }

    void LoseGame()
    {
        gameEnded = true;

        if (gameOverText != null) gameOverText.text = "You Lose!";
        if (finalScoreText != null) finalScoreText.text = "Score: " + score;

        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        TrunkRotate trunk = FindObjectOfType<TrunkRotate>();
        if (trunk != null)
            trunk.ResetTrunk();
    }

    // --- Buttons Methods ---
    public void RestartLevel()
    {
        StartLevel(level);
        gameEnded = false;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void NextLevel()
    {
        level++;
        if (level > 5)
        {
            if (gameOverText != null) gameOverText.text = "🏆 You Finished All Levels!";
            return;
        }

        StartLevel(level);
        gameEnded = false;
    }

    // --- Level Setup ---
    void StartLevel(int currentLevel)
    {
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        gameEnded = false;

        switch (currentLevel)
        {
            case 1: knivesLeft = 5; SetTrunkSpeed(100f); break;
            case 2: knivesLeft = 6; SetTrunkSpeed(150f); break;
            case 3: knivesLeft = 7; SetTrunkSpeed(200f); break;
            case 4: knivesLeft = 8; SetTrunkSpeed(250f); break;
            case 5: knivesLeft = 9; SetTrunkSpeed(300f); break;
            default: knivesLeft = 5; break;
        }

        UpdateUI();
    }

    void SetTrunkSpeed(float speed)
    {
        TrunkRotate trunk = FindObjectOfType<TrunkRotate>();
        if (trunk != null)
            trunk.rotateSpeed = speed;
    }

    void ResetTrunk()
    {
        TrunkRotate trunk = FindObjectOfType<TrunkRotate>();
        if (trunk != null)
            trunk.ResetTrunk();
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (knivesLeftText != null) knivesLeftText.text = "Knives Left: " + knivesLeft;
        if (levelText != null) levelText.text = "Level: " + level;
    }
}
