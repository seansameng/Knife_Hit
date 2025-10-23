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

            transform.SetParent(collision.transform);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

            speed = 0f;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
                GameManager.Instance.UseKnife();
            }

            TrunkRotate trunk = collision.GetComponent<TrunkRotate>();
            if (trunk != null)
            {
                trunk.OnKnifeHit();
            }
        }
        else if (collision.CompareTag("Knife"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.KnifeHitKnife();
        }
    }

    public void ResetKnife(Vector3 spawnPosition)
    {
        isHit = false;
        transform.SetParent(null);
        transform.position = spawnPosition;

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
