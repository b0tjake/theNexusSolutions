// ThrowMeterUI.cs
using UnityEngine;
using UnityEngine.UI;

public class ThrowMeterUI : MonoBehaviour
{
    public PlayerInteract playerInteract;
    public Slider chargeSlider;
    public GameObject sliderContainer; // parent object to show/hide

    void Update()
    {
        bool show = playerInteract.IsHoldingObject && playerInteract.IsCharging;
        sliderContainer.SetActive(show);

        if (show)
            chargeSlider.value = playerInteract.ChargePercent;
    }
}