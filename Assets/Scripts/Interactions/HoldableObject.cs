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

    [Header("Reading Settings")]
    public float readDistance = 1f;
    public Vector3 readRotationEuler = new Vector3(0f, 0f, -90f);

    [Header("Throw Settings")]
    public float minThrowForce = 5f;
    public float maxThrowForce = 25f;

    Rigidbody rb;

    

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
}

    public override void OnInteract()
    {
        switch (currentState)
        {
            case State.Idle:
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
    col.enabled = false;
}

public void PickUp()
{
    currentState = State.Held;
    rb.isKinematic = false;
    rb.useGravity = false;
    rb.linearDamping = 10f;
    rb.angularDamping = 10f;
    col.enabled = false;
}

public void PutBack()
{
    currentState = State.Idle;
    rb.isKinematic = false;
    rb.useGravity = true;
    rb.linearDamping = 1f;
    rb.angularDamping = 0.05f;
    col.enabled = true;

    transform.position = originalPosition;
    transform.rotation = originalRotation;
    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;
}

public void Drop()
{
    currentState = State.Idle;
    rb.useGravity = true;
    rb.linearDamping = 1f;
    rb.angularDamping = 0.05f;
    col.enabled = true;

    transform.rotation = originalRotation;
}

public void Throw(Vector3 direction, float chargePercent)
{
    currentState = State.Idle;
    rb.useGravity = true;
    rb.linearDamping = 1f;
    rb.angularDamping = 0.05f;
    col.enabled = true;

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

            Quaternion targetRot = cameraTransform.rotation * Quaternion.Euler(heldRotationEuler);

            // 🔥 ROTATE CHILD SMOOTHLY
            visual.rotation = Quaternion.Slerp(
                visual.rotation,
                targetRot,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    public bool IsHeld() => currentState == State.Held;
    public bool IsReading() => currentState == State.Reading;
}