using UnityEngine;

public class HoldableObject : Interactable
{
[Header("References")]
public Transform visual;
Collider col;

    [Header("Hold Settings")]
    public float holdDistance = 2f;
    public float followSpeed = 15f;
    public float rotationSpeed = 8f;
    public Vector3 heldRotationEuler = new Vector3(0f, 0f, -90f);
    public bool lockRotationWhenHeld = false; // ← add this


    [Header("Reading Settings")]
    public float readDistance = 1f;
    public Vector3 readRotationEuler = new Vector3(0f, 0f, -90f);
    public bool skipReadingState = false;


    [Header("Throw Settings")]
    public float minThrowForce = 5f;
    public float maxThrowForce = 25f;

    Rigidbody rb;

    int originalLayer;


    public enum State { Idle, Reading, Held }
    public State currentState = State.Idle;

    Vector3 originalPosition;
    Quaternion originalRotation;

    void Start()
{
    rb = GetComponent<Rigidbody>();
    col = GetComponent<Collider>();

    if (visual == null)
        visual = transform.GetChild(0);

    originalPosition = transform.position;
    originalRotation = transform.rotation;
    originalLayer = gameObject.layer;
}

public override void OnInteract()
{
    switch (currentState)
    {
        case State.Idle:
            if (skipReadingState)
                PickUp();
            else
                StartReading();
            break;

        case State.Reading:
            PickUp();
            break;
    }
}

public void StartReading()
{
    currentState = State.Reading;
    rb.linearVelocity = Vector3.zero;
    rb.useGravity = false;
    rb.isKinematic = true;
    gameObject.layer = LayerMask.NameToLayer("HeldItem");
}

public void PickUp()
{
    currentState = State.Held;
    rb.isKinematic = false;
    rb.useGravity = false;
    rb.linearDamping = 10f;
    rb.angularDamping = 10f;
    gameObject.layer = LayerMask.NameToLayer("HeldItem");
}

public void PutBack()
{
    currentState = State.Idle;
    gameObject.layer = originalLayer;

    transform.position = originalPosition;
    transform.rotation = originalRotation;

    rb.isKinematic = true; // kinematic first to teleport cleanly
    rb.isKinematic = false; // then release
    rb.useGravity = true;
    rb.linearDamping = 1f;
    rb.angularDamping = 0.05f;
}
public void Drop()
{
    currentState = State.Idle;
    rb.useGravity = true;
    rb.linearDamping = 1f;
    rb.angularDamping = 0.05f;
    gameObject.layer = originalLayer;

    transform.rotation = originalRotation;
}

public void Throw(Vector3 direction, float chargePercent)
{
    currentState = State.Idle;
    rb.useGravity = true;
    rb.linearDamping = 1f;
    rb.angularDamping = 0.05f;
    gameObject.layer = originalLayer;

    transform.rotation = originalRotation;

    float force = Mathf.Lerp(minThrowForce, maxThrowForce, chargePercent);
    rb.AddForce(direction * force, ForceMode.Impulse);
}

    public void HoldUpdate(Transform cameraTransform)
    {
        if (currentState == State.Reading)
        {
            Vector3 targetPos = cameraTransform.position
                              + cameraTransform.forward * readDistance;

            transform.position = targetPos;

            // 🔥 ROTATE CHILD ONLY
            Quaternion targetRot = cameraTransform.rotation * Quaternion.Euler(readRotationEuler);
            visual.localRotation = Quaternion.Euler(0f, 180f, -90f);        }
        else if (currentState == State.Held)
{
    Vector3 targetPos = cameraTransform.position
                      + cameraTransform.forward * holdDistance;
    rb.linearVelocity = (targetPos - transform.position) * followSpeed;

    if (!lockRotationWhenHeld) // ← only rotate if allowed
    {
        Quaternion targetRot = cameraTransform.rotation * Quaternion.Euler(heldRotationEuler);
        Quaternion newRot = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        rb.MoveRotation(newRot);
    }
}
    }

    public bool IsHeld() => currentState == State.Held;
    public bool IsReading() => currentState == State.Reading;
}