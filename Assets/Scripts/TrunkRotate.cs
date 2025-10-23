using UnityEngine;

public class TrunkRotate : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotateSpeed = 100f;               // Trunk rotation speed
    private bool isRotating = true;

    [Header("Trunk Pieces for Breaking")]
    public Rigidbody2D[] trunkPieces;              // Assign child pieces in inspector
    public int hitTarget = 5;                      // How many knife hits before breaking
    public AudioClip breakSound;                   // Optional: sound when trunk breaks

    private int currentHits = 0;                   // Counts knife hits
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && breakSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isRotating)
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    // Call this method whenever a knife hits the trunk
    public void OnKnifeHit()
    {
        currentHits++;

        if (currentHits >= hitTarget)
        {
            BreakTrunk();
        }
    }

    public void ResetTrunk()
    {
        isRotating = true;
        currentHits = 0;
        transform.rotation = Quaternion.identity;

        foreach (Rigidbody2D piece in trunkPieces)
        {
            piece.isKinematic = true;                  // Stop physics
            piece.linearVelocity = Vector2.zero;
            piece.angularVelocity = 0f;
            piece.transform.localPosition = Vector3.zero; // Reset relative position
        }
    }

    public void BreakTrunk()
    {
        isRotating = false;

        // Play sound
        if (breakSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(breakSound);
        }

        foreach (Rigidbody2D piece in trunkPieces)
        {
            piece.isKinematic = false; // Enable physics
            piece.AddTorque(Random.Range(-200f, 200f)); // Random spin
            piece.AddForce(new Vector2(Random.Range(-50f, 50f), Random.Range(50f, 100f))); // Random burst
        }
    }
}
