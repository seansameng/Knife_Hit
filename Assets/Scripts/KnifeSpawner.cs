using UnityEngine;

public class KnifeSpawner : MonoBehaviour
{
    public GameObject knifePrefab;
    public Transform spawnPoint;

    private GameObject currentKnife;
    private bool knifeReady = false;

    void Start()
    {
        SpawnKnife();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && knifeReady)
        {
            ThrowKnife();
        }
    }

    void SpawnKnife()
    {
        if (knifePrefab == null || spawnPoint == null) return;

        currentKnife = Instantiate(knifePrefab, spawnPoint.position, spawnPoint.rotation);
        currentKnife.transform.localScale = knifePrefab.transform.localScale;
        knifeReady = true;
    }

    void ThrowKnife()
    {
        if (currentKnife == null) return;

        Rigidbody2D rb = currentKnife.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.up * 25f;

        knifeReady = false;
        currentKnife = null;

        Invoke(nameof(SpawnKnife), 0.7f);
    }
}
