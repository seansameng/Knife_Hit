using UnityEngine;
using UnityEngine.UI;

public class KnifeSpawner : MonoBehaviour
{
    [Header("Knife Setup")]
    public GameObject knifePrefab;
    public Transform spawnPoint;
    public float throwForce = 25f;
    public float spawnDelay = 0.7f;
    public int totalKnives = 10; // total knives player starts with

    [Header("UI")]
    public Text scoreText;
    public Text knivesLeftText;

    private GameObject currentKnife;
    private bool knifeReady = false;
    private int score = 0;

    void Start()
    {
        UpdateUI();
        SpawnKnife();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && knifeReady && totalKnives > 0)
        {
            ThrowKnife();
        }
    }

    void SpawnKnife()
    {
        if (knifePrefab == null || spawnPoint == null || totalKnives <= 0) return;

        currentKnife = Instantiate(knifePrefab, spawnPoint.position, spawnPoint.rotation);
        currentKnife.transform.localScale = knifePrefab.transform.localScale;

        // Make sure Knife.cs is attached
        if (currentKnife.GetComponent<Knife>() == null)
            currentKnife.AddComponent<Knife>();

        knifeReady = true;
    }

    void ThrowKnife()
    {
        if (currentKnife == null) return;

        Rigidbody2D rb = currentKnife.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.up * throwForce;

        knifeReady = false;
        currentKnife = null;

        totalKnives--; // decrease knife count
        UpdateUI();

        if (totalKnives > 0)
            Invoke(nameof(SpawnKnife), spawnDelay);
        else
            Debug.Log("Game Over!"); // optional: add Game Over UI later
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "" + score;

        if (knivesLeftText != null)
            knivesLeftText.text = "Knives Left: " + totalKnives;
    }
}
