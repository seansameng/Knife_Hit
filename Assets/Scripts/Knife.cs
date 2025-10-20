using UnityEngine;

public class Knife : MonoBehaviour
{
    public float speed = 15f;
    private bool isHit = false; // prevents multiple hits

    void Update()
    {
        if (!isHit)
            transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHit && collision.CompareTag("Trunk"))
        {
            isHit = true; // mark it as hit
            transform.SetParent(collision.transform);

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Kinematic;

            speed = 0f;

            // Add score
            KnifeSpawner spawner = FindObjectOfType<KnifeSpawner>();
            if (spawner != null)
                spawner.AddScore(1);
        }
    }
}
