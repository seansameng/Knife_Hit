using UnityEngine;
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

    [Header("Prefabs & Spawn Points")]
    public GameObject trunkPrefab;
    public Transform trunkSpawnPoint;
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;

    private TrunkRotate currentTrunk;
    private Knife currentKnife;

    [Header("Game Settings")]
    public int level = 1;
    public int maxLevel = 5;
    public int score = 0;
    private int knivesLeft;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ShowStartPanel();
    }

    void ShowStartPanel()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        score = 0;
        level = 1;
        knivesLeft = 5;
        gameEnded = false;
        UpdateUI();

        DestroyCurrentTrunk();
        DestroyCurrentKnife();
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);

        gameEnded = false;
        score = 0;
        level = 1;
        knivesLeft = 5;
        UpdateUI();

        SpawnTrunk();
        SpawnKnife();
    }

    void SpawnTrunk()
    {
        DestroyCurrentTrunk();
        if (trunkPrefab != null && trunkSpawnPoint != null)
        {
            GameObject trunkObj = Instantiate(trunkPrefab, trunkSpawnPoint.position, Quaternion.identity);
            currentTrunk = trunkObj.GetComponent<TrunkRotate>();
        }
    }

    void SpawnKnife()
    {
        DestroyCurrentKnife();
        if (knifePrefab != null && knifeSpawnPoint != null)
        {
            GameObject knifeObj = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);
            currentKnife = knifeObj.GetComponent<Knife>();
            currentKnife?.ResetKnife();
        }
    }

    void DestroyCurrentTrunk()
    {
        if (currentTrunk != null) Destroy(currentTrunk.gameObject);
    }

    void DestroyCurrentKnife()
    {
        if (currentKnife != null) Destroy(currentKnife.gameObject);
    }

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

        if (knivesLeft <= 0) StartCoroutine(WinLevelFlow());
        else SpawnKnife();
    }

    public void KnifeHitKnife()
    {
        if (!gameEnded) LoseGame();
    }

    IEnumerator WinLevelFlow()
    {
        gameEnded = true;
        UpdateGameOverUI("You Win!");
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        currentTrunk?.BreakTrunk();
        yield return new WaitForSeconds(1.5f);

        DestroyCurrentTrunk();
        DestroyCurrentKnife();

        level++;
        if (level > maxLevel)
        {
            UpdateGameOverUI("🏆 You Finished All Levels!");
            yield break;
        }

        knivesLeft = 5 + level - 1;
        gameEnded = false;

        SpawnTrunk();
        SpawnKnife();

        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        UpdateUI();
    }

    void LoseGame()
    {
        gameEnded = true;
        UpdateGameOverUI("You Lose!");
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        gameEnded = false;
        knivesLeft = 5 + level - 1;

        SpawnTrunk();
        SpawnKnife();

        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        UpdateUI();
    }

    public void QuitToStartPanel()
    {
        ShowStartPanel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
        knivesLeftText.text = $"Knives Left: {knivesLeft}";
        levelText.text = $"Level: {level}";
    }

    void UpdateGameOverUI(string message)
    {
        gameOverText.text = message;
        finalScoreText.text = $"Score: {score}";
    }
}
