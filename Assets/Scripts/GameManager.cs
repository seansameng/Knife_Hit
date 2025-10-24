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
    public TextMeshProUGUI tapToStartText;

    [Header("Prefabs & Spawn Points")]
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;
    public GameObject trunkPrefab;
    public Transform trunkSpawnPoint;

    [Header("Audio")]
    public AudioClip knifeHitSound;
    public AudioClip knifeHitFailSound;
    public AudioClip trunkBreakSound;
    public AudioClip startLevelSound;

    private AudioSource audioSource;

    [Header("Game Stats")]
    public int level = 1;
    public int score = 0;
    public int knivesPerLevel = 5;

    private int knivesLeft;
    private bool waitingForFirstTap = false;
    private bool gameStarted = false;
    private bool gameEnded = false;

    private GameObject currentKnife;
    [HideInInspector] public TrunkRotate currentTrunk;
    private CanvasGroup tapCanvasGroup;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        if (tapToStartText != null)
            tapCanvasGroup = tapToStartText.GetComponent<CanvasGroup>();

        ShowStartPanel();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (waitingForFirstTap && !gameEnded)
            {
                waitingForFirstTap = false;
                gameStarted = true;
                StartCoroutine(FadeOutTapText());
            }
            else if (gameStarted && !gameEnded)
            {
                ThrowKnife();
            }
        }
    }

    // ================= PANEL CONTROL =================
    void ShowStartPanel()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        score = 0;
        level = 1;
        gameEnded = false;
    }

    public void OnPlayButtonPressed()
    {
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(true);

        knivesLeft = knivesPerLevel;
        UpdateUI();

        SpawnTrunk();
        SpawnKnife();

        waitingForFirstTap = true;
        gameStarted = false;

        if (tapCanvasGroup != null)
        {
            tapToStartText.gameObject.SetActive(true);
            tapCanvasGroup.alpha = 1f;
        }

        PlaySound(startLevelSound);
    }

    IEnumerator FadeOutTapText()
    {
        float duration = 0.8f;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            tapCanvasGroup.alpha = Mathf.Lerp(1f, 0f, time / duration);
            yield return null;
        }
        tapToStartText.gameObject.SetActive(false);
    }

    // ================= SPAWNERS =================
    void SpawnTrunk()
    {
        if (currentTrunk != null)
            Destroy(currentTrunk.gameObject);

        GameObject t = Instantiate(trunkPrefab, trunkSpawnPoint.position, Quaternion.identity);
        currentTrunk = t.GetComponent<TrunkRotate>();
        currentTrunk.Setup(level);
    }

    void SpawnKnife()
    {
        if (knifePrefab == null || knifeSpawnPoint == null) return;
        currentKnife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);
    }

    void ThrowKnife()
    {
        if (currentKnife == null) return;

        Rigidbody2D rb = currentKnife.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.up * 25f;
    }

    // ================= GAME LOGIC =================
    public void OnKnifeHitTrunk()
    {
        score++;
        knivesLeft--;
        PlaySound(knifeHitSound);
        UpdateUI();

        if (knivesLeft <= 0)
        {
            StartCoroutine(WinSequence());
        }
        else
        {
            Invoke(nameof(SpawnKnife), 0.6f);
        }
    }

    public void OnKnifeHitKnife()
    {
        if (!gameEnded)
        {
            PlaySound(knifeHitFailSound);
            StartCoroutine(LoseSequence());
        }
    }

    public IEnumerator WinSequence()
    {
        if (gameEnded) yield break;
        gameEnded = true;

        // Play sound safely
        PlaySound(trunkBreakSound);

        // Only break if the trunk still exists
        if (currentTrunk != null)
            currentTrunk.BreakTrunk();

        yield return new WaitForSeconds(1.5f);

        level++;
        knivesLeft = knivesPerLevel + level - 1;
        gameEnded = false;
        UpdateUI();

        SpawnTrunk();
        SpawnKnife();

        waitingForFirstTap = true;
        gameStarted = false;

        if (tapCanvasGroup != null)
        {
            tapToStartText.gameObject.SetActive(true);
            tapCanvasGroup.alpha = 1f;
        }
    }

    IEnumerator LoseSequence()
    {
        gameEnded = true;
        yield return new WaitForSeconds(1f);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gameOverText.text = "You Lose!";
        finalScoreText.text = $"Score: {score}";
    }

    public void OnRestartButtonPressed()
    {
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(true);
        knivesLeft = knivesPerLevel + level - 1;
        gameEnded = false;
        score = 0;

        SpawnTrunk();
        SpawnKnife();

        waitingForFirstTap = true;
        gameStarted = false;

        if (tapCanvasGroup != null)
        {
            tapToStartText.gameObject.SetActive(true);
            tapCanvasGroup.alpha = 1f;
        }
    }

    // ================= HELPERS =================
    void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
        knivesLeftText.text = $"Knives Left: {knivesLeft}";
        levelText.text = $"Level: {level}";
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
