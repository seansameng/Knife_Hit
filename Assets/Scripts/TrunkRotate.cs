using UnityEngine;

public class TrunkRotate : MonoBehaviour
{
    public float rotateSpeed = 150f;
    public int hitTarget = 5;
    public Rigidbody2D[] trunkPieces;

    private int currentHits = 0;
    private bool isBroken = false;
    private bool stopped = false;

    void Update()
    {
        if (!isBroken && !stopped)
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    public void OnKnifeHit()
    {
        if (isBroken || stopped) return;

        currentHits++;
        GameManager.Instance?.AddScore(1);
        GameManager.Instance?.PlayKnifeHitSound();

        if (currentHits >= hitTarget)
        {
            isBroken = true;
            GameManager.Instance?.OnTrunkBroken();
        }
    }

    public void BreakTrunk()
    {
        GameManager.Instance?.PlayTrunkBreakSound();

        foreach (Rigidbody2D piece in trunkPieces)
        {
            if (piece == null) continue;
            piece.transform.SetParent(null);
            piece.isKinematic = false;
        }

        gameObject.SetActive(false);
    }

    public void StopTrunk()
    {
        stopped = true;
    }
}
