using UnityEngine;

public class Knife : MonoBehaviour
{
    public float speed = 15f;
    private bool thrown = false;
    private bool stuck = false;

    void Update()
    {
        if (thrown && !stuck)
            transform.Translate(Vector2.up * speed * Time.deltaTime);

        if (!thrown && Input.GetMouseButtonDown(0))
            thrown = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (stuck) return;

        if (collision.CompareTag("Trunk"))
        {
            stuck = true;
            thrown = false;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            transform.SetParent(collision.transform);
            speed = 0f;

            collision.GetComponent<TrunkRotate>()?.OnKnifeHit();
            GameManager.Instance?.UseKnife();
        }
        else if (collision.CompareTag("Knife"))
        {
            stuck = true;
            thrown = false;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            GameManager.Instance?.KnifeHitKnife();
        }
    }
}
