using UnityEngine;

public class Knife : MonoBehaviour
{
    public float speed = 15f;
    private bool isHit = false;

    void Update()
    {
        if (!isHit)
            transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit) return; // prevent multiple hits

        if (collision.CompareTag("Trunk"))
        {
            isHit = true;

            transform.SetParent(collision.transform);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Kinematic;
            speed = 0f;

            // Add score + decrease knives left
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
                GameManager.Instance.UseKnife();
            }
        }
        else if (collision.CompareTag("Knife"))
        {
            // Knife hit another knife → lose
            if (GameManager.Instance != null)
            {
                GameManager.Instance.KnifeHitKnife();
            }
        }
    }
}
