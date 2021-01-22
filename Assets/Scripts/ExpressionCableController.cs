using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExpressionCableController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    FacePlate facePlate;
    FaceCablePlug curCable;

    private void Start()
    {
        facePlate = FindObjectOfType<FacePlate>();
    }

    private void LateUpdate()
    {
        if (curCable == null)
            return;

        curCable.FollowMouse();
    }

    public void ResetCable()
    {
        curCable = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var other = eventData.pointerEnter.gameObject;
        var cable = other.GetComponentInParent<FaceCablePlug>();
        
        if (cable == null)
            return;

        var expression = cable.Expression;
        switch (other.tag)
        {
            case "Plug":
                if (cable.IsBroken)
                    curCable = cable;
                break;

            case "Outlet":
                if (curCable == null)
                    return;

                // Are the same - connect them
                if (curCable.Expression == expression)
                    facePlate.FixExpression(expression);

                ResetCable();
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerClick(eventData);
    }
}
