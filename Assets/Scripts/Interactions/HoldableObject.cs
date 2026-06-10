// HoldableObject.cs
using UnityEngine;

public class HoldableObject : Interactable
{
    [Header("Hold Settings")]
    public float holdDistance = 2f;       // how far in front of camera
    public float followSpeed = 15f;       // how smoothly it follows
    public float rotationSpeed = 8f;

    Rigidbody rb;
    bool isHeld = false;
    Transform holdTarget;                 // empty transform in front of camera

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
        holdTarget = null;
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