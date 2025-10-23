using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

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
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI finalScoreText;

    [Header("Prefabs & Spawns")]
    public GameObject trunkPrefab;       // optional, only used if no trunk exists
    public Transform trunkSpawnPoint;
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;

    private TrunkRotate currentTrunk;
    private bool gameEnded = false;

    [Header("Level Settings")]
    public int level = 1;
    public int score = 0;
    private int knivesLeft;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        ShowStartPanel();
    }

    void ShowStartPanel()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        score = 0;
        level = 1;
        gameEnded = false;
        UpdateUI();
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

        FindExistingTrunk(); // ✅ Find or create trunk
        StartLevel(level);
    }

    void FindExistingTrunk()
    {
        // Look for trunk already placed in scene
        currentTrunk = FindObjectOfType<TrunkRotate>();

        // If no trunk in scene, spawn one
        if (currentTrunk == null && trunkPrefab != null && trunkSpawnPoint != null)
        {
            GameObject trunkObj = Instantiate(trunkPrefab, trunkSpawnPoint.position, Quaternion.identity);
            currentTrunk = trunkObj.GetComponent<TrunkRotate>();
        }
    }

    // --- Knife Logic ---
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
            StartCoroutine(WinLevelFlow());
        }
    }

    public void KnifeHitKnife()
    {
        if (!gameEnded)
            LoseGame();
    }

    // --- Win/Lose Logic ---
    IEnumerator WinLevelFlow()
    {
        gameEnded = true;

        UpdateGameOverUI("You Win!");
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        if (currentTrunk != null)
            currentTrunk.BreakTrunk();

        yield return new WaitForSeconds(2f);

        ResetKnives();

        level++;
        if (level > 5)
        {
            UpdateGameOverUI("🏆 You Finished All Levels!");
            yield break;
        }

        // reuse existing trunk or respawn if destroyed
        if (currentTrunk == null)
            FindExistingTrunk();

        StartLevel(level);
        gameEnded = false;
    }
    void LoseGame()
    {
        gameEnded = true;
        UpdateGameOverUI("You Lose!");
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
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
        SpawnKnife();
    }

    void SpawnKnife()
    {
        if (knifePrefab != null && knifeSpawnPoint != null)
        {
            Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);
        }
    }

    void ResetKnives()
    {
        foreach (Knife k in FindObjectsOfType<Knife>())
        {
            Destroy(k.gameObject);
        }
    }

    void SetTrunkSpeed(float speed)
    {
        if (currentTrunk != null)
            currentTrunk.rotateSpeed = speed;
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"Score: {score}";
        if (knivesLeftText != null) knivesLeftText.text = $"Knives Left: {knivesLeft}";
        if (levelText != null) levelText.text = $"Level: {level}";
    }

    void UpdateGameOverUI(string message)
    {
        if (gameOverText != null) gameOverText.text = message;
        if (finalScoreText != null) finalScoreText.text = $"Score: {score}";
    }

    // --- Buttons ---
    public void RestartLevel()
    {
        ResetKnives();

        if (currentTrunk != null)
        {
            Destroy(currentTrunk.gameObject);
            currentTrunk = null;
        }

        FindExistingTrunk();
        StartLevel(level);
        gameEnded = false;
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void QuitToStartPanel()
    {
        ShowStartPanel();
        ResetKnives();

        if (currentTrunk != null)
        {
            Destroy(currentTrunk.gameObject);
            currentTrunk = null;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}




