using UnityEngine;

public class KnifeSpawner : MonoBehaviour
{
    [Header("Knife Setup")]
    public GameObject knifePrefab; // assign Knife prefab
    public Transform spawnPoint;   // assign SpawnPoint

    private GameObject currentKnife;
    private bool knifeReady = false;

    void Start()
    {
        // Spawn the first knife (but don't throw automatically)
        SpawnKnife();
    }

    void Update()
    {
        // Only throw when player clicks and knife is ready
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
        currentKnife.transform.localScale = knifePrefab.transform.localScale;
        knifeReady = true;
    }

    void ThrowKnife()
    {
        if (currentKnife == null) return;

        Rigidbody2D rb = currentKnife.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.up * 25f; // controls how fast the knife flies

        knifeReady = false;
        currentKnife = null;

        // spawn the next knife after 0.7 seconds
        Invoke(nameof(SpawnKnife), 0.7f);



    }
}
