using UnityEngine;
using System.Collections;

public class TrunkRotate : MonoBehaviour
{
    public float rotateSpeed = 150f;
    public int hitTarget = 5;
    public Rigidbody2D[] trunkPieces;

    private int hits = 0;
    private bool isBroken = false;

    void Update()
    {
        if (!isBroken)
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    public void Setup(int level)
    {
        hitTarget = 5 + (level - 1);
        isBroken = false;
        hits = 0;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);
    }

    public void OnKnifeHit()
    {
        if (isBroken) return;

        hits++;
        GameManager.Instance.OnKnifeHitTrunk();

        if (hits >= hitTarget)
        {
            isBroken = true;
            StartCoroutine(BreakDelay());
        }
    }

    private IEnumerator BreakDelay()
    {
        yield return new WaitForSeconds(0.15f);

        // Tell GameManager to handle the win — it will break the trunk safely
        if (GameManager.Instance != null)
            StartCoroutine(GameManager.Instance.WinSequence());
    }

    public void BreakTrunk()
    {
        foreach (Rigidbody2D piece in trunkPieces)
        {
            if (piece == null) continue;
            piece.transform.SetParent(null);
            piece.isKinematic = false;
            piece.AddForce(Random.insideUnitCircle * 5f, ForceMode2D.Impulse);
            piece.AddTorque(Random.Range(-200f, 200f));
        }

        gameObject.SetActive(false);
    }
}
