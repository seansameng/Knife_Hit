using UnityEngine;

public class TrunkRotate : MonoBehaviour
{
    public float rotateSpeed = 120f;
    public bool clockwise = true;

    private float initialRotation;

    void Start()
    {
        initialRotation = transform.eulerAngles.z;
    }

    void Update()
    {
        float dir = clockwise ? -1f : 1f;
        transform.Rotate(0, 0, rotateSpeed * dir * Time.deltaTime);
    }

    // --- Reset trunk rotation ---
    public void ResetTrunk()
    {
        transform.rotation = Quaternion.Euler(0, 0, initialRotation);
    }
}
