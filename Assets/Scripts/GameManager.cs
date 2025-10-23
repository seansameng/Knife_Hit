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

    [Header("Tap To Start Text")]
    public TextMeshProUGUI tapToStartText;

    [Header("Audio Clips")]
    public AudioClip trunkBreakSound;
    public AudioClip knifeHitSound;
    public AudioClip knifeHitFailSound;
    public AudioClip startLevelSound;

    private AudioSource audioSource;
    private CanvasGroup tapTextCanvasGroup;
    private TrunkRotate currentTrunk;
    private GameObject currentKnife;
    private Rigidbody2D currentKnifeRb;

    [Header("Game Settings")]
    public int level = 1;
    public int maxLevel = 5;
    public int score = 0;
    private int knivesLeft;
    private bool knifeReady = false;
    private bool waitingForFirstClick = false;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (tapToStartText != null)
            tapTextCanvasGroup = tapToStartText.GetComponent<CanvasGroup>();

        ShowStartPanel();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (waitingForFirstClick && !gameEnded)
            {
                waitingForFirstClick = false;
                knifeReady = true;
                StartCoroutine(FadeOutTapToStart());
            }
            else if (knifeReady && !gameEnded)
            {
                ThrowKnife();
            }
        }
    }

    // ---------------- PANEL CONTROL ----------------

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

    public void PlayButtonPressed()
    {
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(true);

        waitingForFirstClick = true;
        gameEnded = false;
        knifeReady = false;

        SpawnTrunk();
        SpawnKnife();
        UpdateUI();

        PlayStartLevelSound();

        if (tapTextCanvasGroup != null)
        {
            tapTextCanvasGroup.alpha = 1f;
            tapToStartText.gameObject.SetActive(true);
        }
    }

    // ---------------- SPAWNING ----------------

    void SpawnTrunk()
    {
        DestroyCurrentTrunk();

        if (trunkPrefab && trunkSpawnPoint)
        {
            GameObject trunkObj = Instantiate(trunkPrefab, trunkSpawnPoint.position, Quaternion.identity);
            trunkObj.name = "Trunk";
            currentTrunk = trunkObj.GetComponent<TrunkRotate>();
        }
    }

    void SpawnKnife()
    {
        DestroyCurrentKnife();

        if (knifePrefab && knifeSpawnPoint)
        {
            currentKnife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);
            currentKnifeRb = currentKnife.GetComponent<Rigidbody2D>();
            knifeReady = false;
        }
    }

    void ThrowKnife()
    {
        if (currentKnife == null) return;

        currentKnifeRb.linearVelocity = Vector2.up * 25f;
        knifeReady = false;
    }

    // ---------------- GAME EVENTS ----------------

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
        else
        {
            Invoke(nameof(SpawnKnife), 0.6f);
        }
    }

    public void KnifeHitKnife()
    {
        if (!gameEnded)
        {
            PlayKnifeHitFailSound();
            LoseGame();
        }
    }

    public void OnTrunkBroken()
    {
        if (gameEnded) return;
        PlayTrunkBreakSound();
        StartCoroutine(WinLevelFlow());
    }

    IEnumerator WinLevelFlow()
    {
        gameEnded = true;
        UpdateGameOverUI("You Win!");
        currentTrunk?.BreakTrunk();

        yield return new WaitForSeconds(1.5f);

        DestroyCurrentTrunk();
        DestroyCurrentKnife();

        level++;
        if (level > maxLevel)
        {
            UpdateGameOverUI("🏆 You Finished All Levels!");
            gamePanel.SetActive(false);
            gameOverPanel.SetActive(true);
            yield break;
        }

        knivesLeft = 5 + level - 1;
        gameEnded = false;

        SpawnTrunk();
        SpawnKnife();

        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        UpdateUI();

        PlayStartLevelSound();
    }

    void LoseGame()
    {
        gameEnded = true;
        UpdateGameOverUI("You Lose!");

        if (currentTrunk != null)
            currentTrunk.StopTrunk();

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

        PlayStartLevelSound();
    }

    public void QuitToStartPanel()
    {
        ShowStartPanel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // ---------------- CLEANUP + UI ----------------

    void DestroyCurrentTrunk()
    {
        if (currentTrunk != null)
            Destroy(currentTrunk.gameObject);
    }

    void DestroyCurrentKnife()
    {
        if (currentKnife != null)
            Destroy(currentKnife.gameObject);
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

    IEnumerator FadeOutTapToStart()
    {
        if (tapTextCanvasGroup == null) yield break;

        float duration = 0.8f;
        float startAlpha = tapTextCanvasGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            tapTextCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            yield return null;
        }

        tapTextCanvasGroup.alpha = 0f;
        tapToStartText.gameObject.SetActive(false);
    }

    // ---------------- SOUND SYSTEM ----------------

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }

    public void PlayTrunkBreakSound() => PlaySound(trunkBreakSound);
    public void PlayKnifeHitSound() => PlaySound(knifeHitSound);
    public void PlayKnifeHitFailSound() => PlaySound(knifeHitFailSound);
    public void PlayStartLevelSound() => PlaySound(startLevelSound);
}
