// HoldableObject.cs
using UnityEngine;

public class HoldableObject : Interactable
{
    [Header("Hold Settings")]
    public float holdDistance = 2f;
    public float followSpeed = 15f;
    public float rotationSpeed = 8f;

    [Header("Throw Settings")]
    public float minThrowForce = 5f;
    public float maxThrowForce = 25f;

    Rigidbody rb;
    bool isHeld = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnInteract()
    {
        if (!isHeld)
            PickUp();
        else
            Drop();
    }

    public void PickUp()
    {
        isHeld = true;
        rb.useGravity = false;
        rb.linearDamping = 10f;
        rb.angularDamping = 10f;
    }

    public void Drop()
    {
        isHeld = false;
        rb.useGravity = true;
        rb.linearDamping = 1f;
        rb.angularDamping = 0.05f;
    }

    public void Throw(Vector3 direction, float chargePercent)
    {
        isHeld = false;
        rb.useGravity = true;
        rb.linearDamping = 1f;
        rb.angularDamping = 0.05f;

        float force = Mathf.Lerp(minThrowForce, maxThrowForce, chargePercent);
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    public void HoldUpdate(Transform cameraTransform)
    {
        if (!isHeld) return;

        Vector3 targetPos = cameraTransform.position
                          + cameraTransform.forward * holdDistance;

        rb.linearVelocity = (targetPos - transform.position) * followSpeed;
    }

    public bool IsHeld() => isHeld;
}