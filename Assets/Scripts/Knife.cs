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
        if (isHit) return;

        if (collision.CompareTag("Trunk"))
        {
            isHit = true;

            // Stick knife to trunk
            transform.SetParent(collision.transform);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Kinematic;

            speed = 0f;

            // Add score & decrease knives
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
                GameManager.Instance.UseKnife();
            }

            // Notify trunk of hit
            TrunkRotate trunk = collision.GetComponent<TrunkRotate>();
            if (trunk != null)
            {
                trunk.OnKnifeHit();
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

    // Call this to reset knife for next level
    public void ResetKnife()
    {
        isHit = false;
        transform.SetParent(null);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        speed = 15f;
        gameObject.SetActive(true);
    }
}
