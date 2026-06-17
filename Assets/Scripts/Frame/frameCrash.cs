using UnityEngine;

public class FrameCrash : MonoBehaviour
{
    public GameObject intactGlass;   // enabled at start
    public GameObject brokenGlass;   // disabled at start
    public float breakForce = 2f;

    bool isBroken = false;

    void OnCollisionEnter(Collision collision)
    {
        if (isBroken) return;

        // Only break if hit hard enough
        if (collision.relativeVelocity.magnitude < breakForce) return;

        Break();
    }

    void Break()
    {
        isBroken = true;
        intactGlass.SetActive(false);
        brokenGlass.SetActive(true);
    }
}