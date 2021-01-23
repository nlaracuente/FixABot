using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BatteryPlateButton : RobotButton
{
    BatteryCover batteryCover;

    private void Start()
    {
        batteryCover = FindObjectOfType<BatteryCover>();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        batteryCover.ToggleCoverState();
    }
}
