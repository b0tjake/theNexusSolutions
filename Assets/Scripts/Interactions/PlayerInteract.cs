using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float range = 2f;
    public Camera playerCamera;

    [Header("Throw Settings")]
    public float maxChargeTime = 1.5f;

    Interactable current;
    HoldableObject activeObject; // either Reading or Held

    float chargeTime = 0f;
    bool isCharging = false;

    public float ChargePercent => Mathf.Clamp01(chargeTime / maxChargeTime);
    public bool IsCharging => isCharging;
    public bool IsHoldingObject => activeObject != null && activeObject.IsHeld();

    void Update()
    {
        if (activeObject != null)
            activeObject.HoldUpdate(playerCamera.transform);

        // Raycast for interactables
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, range);

        Interactable found = null;
        foreach (RaycastHit hit in hits)
        {
            found = hit.collider.GetComponent<Interactable>()
                 ?? hit.collider.GetComponentInParent<Interactable>();
            if (found != null) break;
        }
        current = found;

        // E = progress through states
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (activeObject == null && current != null)
            {
                current.OnInteract();
                HoldableObject holdable = current as HoldableObject;
                if (holdable != null)
                    activeObject = holdable;
            }
            else if (activeObject != null)
            {
                activeObject.OnInteract(); // Reading -> Held
            }
        }

        // F = put back (only works while Reading)
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (activeObject != null && activeObject.IsReading())
            {
                activeObject.PutBack();
                activeObject = null;
            }
        }

        // LMB charge + throw (only while Held)
        if (activeObject != null && activeObject.IsHeld())
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

                activeObject.Throw(throwDir, percent);
                activeObject = null;

                isCharging = false;
                chargeTime = 0f;
            }
        }

        Debug.DrawRay(playerCamera.transform.position,
                      playerCamera.transform.forward * range, Color.red);
    }
}