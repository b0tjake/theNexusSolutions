// PaperCrumple.cs
using UnityEngine;

public class PaperCrumple : MonoBehaviour
{
    public Animator paperAnimator;
    public Animator endDoorAnimator;
    public HoldableObject holdable; // drag the PARENT's HoldableObject here
    bool isCrumpled = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && holdable.IsHeld() && !isCrumpled)
        {
            isCrumpled = true;
            paperAnimator.SetBool("IsCrumpled", true);
            endDoorAnimator.SetBool("Open", true);
            Debug.Log("Lbab T7l");

        }
    }
}