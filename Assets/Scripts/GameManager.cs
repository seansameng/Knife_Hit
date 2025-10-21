using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // <-- for TextMeshProUGUI

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI knivesLeftText;
    public TextMeshProUGUI levelText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;   // "You Win!" / "You Lose!"
    public TextMeshProUGUI finalScoreText; // final score

    [Header("Game Panel")]
    public GameObject gamePanel;

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
        gameOverText.text = "You Win!";
        finalScoreText.text = "Score: " + score;
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        ResetTrunk();

        Invoke(nameof(NextLevel), 2f);
    }

    void LoseGame()
    {
        gameEnded = true;
        gameOverText.text = "You Lose!";
        finalScoreText.text = "Score: " + score;
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        ResetTrunk();
    }

    // --- Buttons Methods ---
    public void RestartLevel()
    {
        StartLevel(level);
        gameEnded = false;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // change to your main menu scene name
    }

    void NextLevel()
    {
        level++;
        if (level > 5)
        {
            gameOverText.text = "🏆 You Finished All Levels!";
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

        switch (currentLevel)
        {
            case 1: knivesLeft = 5; SetTrunkSpeed(100f); break;
            case 2: knivesLeft = 6; SetTrunkSpeed(150f); break;
            case 3: knivesLeft = 7; SetTrunkSpeed(200f); break;
            case 4: knivesLeft = 8; SetTrunkSpeed(250f); break;
            case 5: knivesLeft = 9; SetTrunkSpeed(300f); break;
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
            trunk.ResetTrunk(); // now this works
    }


    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
        if (knivesLeftText != null)
            knivesLeftText.text = "Knives Left: " + knivesLeft;
        if (levelText != null)
            levelText.text = "Level: " + level;
    }
}
