using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationArrow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Tooltip("-1 left, 1 right"), Range(-1, 1)]
    int direction = 1;

    RobotStand robotStand;

    private void Start()
    {
        robotStand = FindObjectOfType<RobotStand>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        robotStand.Rotate(direction);
    }
}
