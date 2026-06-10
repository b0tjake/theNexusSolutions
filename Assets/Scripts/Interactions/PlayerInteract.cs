// PlayerInteract.cs
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float range = 2f;
    public Camera playerCamera;

    Interactable current;
    HoldableObject heldObject;

    void Update()
    {
        // If holding something, update its position every frame
        if (heldObject != null)
            heldObject.HoldUpdate(playerCamera.transform);

        // Raycast
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            // If already holding something — drop it
            if (heldObject != null)
            {
                heldObject.Drop();
                heldObject = null;
                return;
            }

            // If looking at something — interact
            if (current != null)
            {
                current.OnInteract();

                // If it's holdable, track it
                HoldableObject holdable = current as HoldableObject;
                if (holdable != null)
                    heldObject = holdable;
            }
        }

        Debug.DrawRay(playerCamera.transform.position, 
                      playerCamera.transform.forward * range, Color.red);
    }
}