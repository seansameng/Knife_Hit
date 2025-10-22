using UnityEngine;

public class TrunkRotate : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotateSpeed = 100f;
    private bool isRotating = true;

    [Header("Trunk Pieces for Breaking")]
    public Rigidbody2D[] trunkPieces; // assign in Inspector

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }
    }

    public void ResetTrunk()
    {
        isRotating = true;
        transform.rotation = Quaternion.identity;

        foreach (Rigidbody2D piece in trunkPieces)
        {
            piece.isKinematic = true; // stop physics
            piece.linearVelocity = Vector2.zero;
            piece.angularVelocity = 0f;
            piece.transform.localPosition = Vector3.zero; // reset relative position
            piece.transform.localRotation = Quaternion.identity;
        }
    }

    public void BreakTrunk()
    {
        isRotating = false;

        foreach (Rigidbody2D piece in trunkPieces)
        {
            piece.isKinematic = false; // physics take over
            piece.AddTorque(Random.Range(-200f, 200f)); // random spin
            piece.AddForce(new Vector2(Random.Range(-50f, 50f), Random.Range(50f, 150f))); // random burst
        }
    }
}
