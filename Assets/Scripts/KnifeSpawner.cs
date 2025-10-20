using UnityEngine;

public class KnifeSpawner : MonoBehaviour
{
    public GameObject knifePrefab; // assign Knife prefab
    public Transform spawnPoint;   // assign SpawnPoint

    private GameObject currentKnife;
    private bool knifeReady = false;

    void Start()
    {
        // Spawn the first knife, but don’t throw
        SpawnKnife();
    }

    void Update()
    {
        // Throw knife only when player clicks
        if (Input.GetMouseButtonDown(0) && knifeReady)
        {
            ThrowKnife();
        }
    }

    void SpawnKnife()
    {
        if (knifePrefab == null || spawnPoint == null)
        {
            Debug.LogError("KnifePrefab or SpawnPoint missing!");
            return;
        }

        currentKnife = Instantiate(knifePrefab, spawnPoint.position, spawnPoint.rotation);
        currentKnife.transform.localScale = knifePrefab.transform.localScale; // keep correct size
        knifeReady = true;
    }

    void ThrowKnife()
    {
        if (currentKnife == null) return;

        Rigidbody2D rb = currentKnife.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.up * 25f; // knife speed
        knifeReady = false;

        currentKnife = null;

        // Spawn next knife after delay
        Invoke(nameof(SpawnKnife), 0.7f);
    }
}
