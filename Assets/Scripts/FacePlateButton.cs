using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FacePlateButton : RobotButton
{
    FacePlate facePlate;
    ExpressionCableController expressionCableController;
     
    private void Start()
    {
        facePlate = FindObjectOfType<FacePlate>();
        expressionCableController = FindObjectOfType<ExpressionCableController>();
    }

    override public void OnPointerClick(PointerEventData eventData)
    {
        facePlate.ToggleFacePlateState();
        expressionCableController.ResetCable();
    }
}
