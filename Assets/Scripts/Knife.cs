using UnityEngine;

public class Knife : MonoBehaviour
{
    public float speed = 15f;
    private bool stuck = false;

    void Update()
    {
        if (!stuck)
            transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (stuck) return;

        if (collision.CompareTag("Trunk"))
        {
            stuck = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;

            transform.SetParent(collision.transform);
            transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));

            collision.GetComponent<TrunkRotate>()?.OnKnifeHit();
        }
        else if (collision.CompareTag("Knife"))
        {
            GameManager.Instance.OnKnifeHitKnife();
        }
    }
}
