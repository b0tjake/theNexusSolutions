// PlayerInteract.cs
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float range = 2f;
    public Camera playerCamera;

    [Header("Throw Settings")]
    public float maxChargeTime = 1.5f; // seconds to reach full power

    Interactable current;
    HoldableObject heldObject;

    float chargeTime = 0f;
    bool isCharging = false;

    // Exposed for UI slider
    public float ChargePercent => Mathf.Clamp01(chargeTime / maxChargeTime);
    public bool IsCharging => isCharging;
    public bool IsHoldingObject => heldObject != null;

    void Update()
    {
        if (heldObject != null)
            heldObject.HoldUpdate(playerCamera.transform);

        // Raycast for interactables
        Ray ray = new Ray(playerCamera.transform.position,
                          playerCamera.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, range);

        Interactable found = null;
        foreach (RaycastHit hit in hits)
        {
            found = hit.collider.GetComponent<Interactable>()
                 ?? hit.collider.GetComponentInParent<Interactable>();
            if (found != null) break;
        }
        current = found;

        // E = pick up / drop
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject != null)
            {
                heldObject.Drop();
                heldObject = null;
                return;
            }

            if (current != null)
            {
                current.OnInteract();
                HoldableObject holdable = current as HoldableObject;
                if (holdable != null)
                    heldObject = holdable;
            }
        }

        // LMB charge + throw
        if (heldObject != null)
        {
            if (Input.GetMouseButton(0))
            {
                isCharging = true;
                chargeTime += Time.deltaTime;
                chargeTime = Mathf.Min(chargeTime, maxChargeTime);
            }

            if (Input.GetMouseButtonUp(0) && isCharging)
            {
                float percent = ChargePercent;
                Vector3 throwDir = playerCamera.transform.forward;

                heldObject.Throw(throwDir, percent);
                heldObject = null;

                isCharging = false;
                chargeTime = 0f;
            }
        }

        Debug.DrawRay(playerCamera.transform.position,
                      playerCamera.transform.forward * range, Color.red);
    }
}