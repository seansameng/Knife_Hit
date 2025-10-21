using System;
using UnityEngine;

public class TrunkRotate : MonoBehaviour
{
    public float rotateSpeed = 120f; // ល្បឿនវិល
    public bool clockwise = true;    // វិលទៅស្ដាំឬឆ្វេង

    internal void ResetTrunk()
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        float dir = clockwise ? -1f : 1f;
        transform.Rotate(0, 0, rotateSpeed * dir * Time.deltaTime);
    }
}
