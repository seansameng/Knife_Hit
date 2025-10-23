using UnityEngine;

public class TrunkRotate : MonoBehaviour
{
    public float rotateSpeed = 150f;
    public Rigidbody2D[] trunkPieces; // assign child pieces in inspector
    public int hitTarget = 5;
    public AudioClip breakSound;

    private int currentHits = 0;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    // Call this when a knife hits the trunk
    public void OnKnifeHit()
    {
        currentHits++;
        if (currentHits >= hitTarget)
        {
            BreakTrunk();
        }
    }

    public void BreakTrunk()
    {
        // Play break sound
        if (audioSource != null && breakSound != null)
            audioSource.PlayOneShot(breakSound);

        // Unparent pieces and enable physics
        foreach (Rigidbody2D piece in trunkPieces)
        {
            piece.transform.SetParent(null);
            piece.isKinematic = false;
        }

        // Disable main trunk object
        gameObject.SetActive(false);
    }

    public void ResetTrunk()
    {
        currentHits = 0;
        foreach (Rigidbody2D piece in trunkPieces)
        {
            piece.transform.SetParent(transform);
            piece.isKinematic = true;
            piece.transform.localPosition = Vector3.zero;
            piece.transform.localRotation = Quaternion.identity;
        }
        gameObject.SetActive(true);
    }
}
