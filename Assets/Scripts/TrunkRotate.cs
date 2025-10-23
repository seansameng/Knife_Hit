using UnityEngine;

public class TrunkRotate : MonoBehaviour
{
    public float rotateSpeed = 100f;
    public Rigidbody2D[] trunkPieces;
    public int hitTarget = 5;
    public AudioClip breakSound;

    private int hitCount = 0;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (trunkPieces.Length == 0)
            trunkPieces = GetComponentsInChildren<Rigidbody2D>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    public void OnKnifeHit()
    {
        hitCount++;
        if (hitCount >= hitTarget)
        {
            BreakTrunk();
        }
    }

    public void BreakTrunk()
    {
        foreach (Rigidbody2D piece in trunkPieces)
        {
            piece.transform.SetParent(null);
            piece.isKinematic = false;
            piece.AddForce(Vector2.up * 200f); // explode effect
        }

        if (audioSource != null && breakSound != null)
            audioSource.PlayOneShot(breakSound);

        Destroy(gameObject, 2f); // destroy after 2 seconds
    }

    public void ResetTrunk()
    {
        hitCount = 0;
        foreach (Rigidbody2D piece in trunkPieces)
        {
            piece.transform.SetParent(transform);
            piece.isKinematic = true;
        }
    }
}
