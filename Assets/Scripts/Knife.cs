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
        if (collision.CompareTag("Trunk"))
        {
            isHit = true;
            transform.SetParent(collision.transform);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            speed = 0f;
        }
    }
}
