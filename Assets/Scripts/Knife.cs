using UnityEngine;

public class Knife : MonoBehaviour
{
    public float speed = 15f;
    private bool isHit = false;
    private bool readyToThrow = false;

    void Update()
    {
        // Only move if thrown
        if (readyToThrow && !isHit)
            transform.Translate(Vector2.up * speed * Time.deltaTime);

        // Click to throw knife
        if (!readyToThrow && Input.GetMouseButtonDown(0))
        {
            readyToThrow = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit) return;

        if (collision.CompareTag("Trunk"))
        {
            isHit = true;
            readyToThrow = false;

            transform.SetParent(collision.transform);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

            speed = 0f;

            // Add score & decrease knives
            GameManager.Instance?.AddScore(1);
            GameManager.Instance?.UseKnife();

            // Notify trunk of hit
            collision.GetComponent<TrunkRotate>()?.OnKnifeHit();
        }
        else if (collision.CompareTag("Knife"))
        {
            GameManager.Instance?.KnifeHitKnife();
        }
    }

    public void ResetKnife()
    {
        isHit = false;
        readyToThrow = false;
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
