using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomOutButton : MonoBehaviour, IPointerClickHandler
{
    CameraController cameraController;

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        cameraController.ZoomOut();
    }
}
