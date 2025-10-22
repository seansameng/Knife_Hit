using UnityEngine;

public class TrunkRotate : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotateSpeed = 100f;
    private bool isRotating = true;

    [Header("Trunk Pieces for Breaking")]
    public Rigidbody2D[] trunkPieces; // assign child pieces in inspector

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
            piece.linearVelocity = Vector2.zero;
            piece.angularVelocity = 0f;
            piece.isKinematic = true;
            piece.transform.localPosition = Vector3.zero; // reset relative position
        }
    }

    public void BreakTrunk()
    {
        isRotating = false;

        foreach (Rigidbody2D piece in trunkPieces)
        {
            piece.isKinematic = false;
            piece.AddTorque(Random.Range(-200f, 200f));
            piece.AddForce(new Vector2(Random.Range(-50f, 50f), Random.Range(50f, 100f)));
        }
    }
}
