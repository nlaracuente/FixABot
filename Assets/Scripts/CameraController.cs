using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Camera currentCamera;

    [SerializeField]
    Camera fullViewCamera;

    [SerializeField]
    Camera zoomedInViewCamera;

    public Camera MainCamera { get { return currentCamera; } }    

    public bool IsZoomedOut { get { return currentCamera == fullViewCamera; } } 

    private void Awake()
    {
        if(currentCamera == null)
            currentCamera = fullViewCamera;

        fullViewCamera.enabled = false;
        zoomedInViewCamera.enabled = false;
        currentCamera.enabled = true;
    }  

    void ChangeCamera(Camera camera)
    {
        currentCamera.enabled = false;
        currentCamera = camera;
        currentCamera.enabled = true;
    }

    public void ZoomOut()
    {
        if (currentCamera == fullViewCamera)
            return;

        ChangeCamera(fullViewCamera);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentCamera != fullViewCamera)
            return;

        ChangeCamera(zoomedInViewCamera);
        zoomedInViewCamera.transform.position = new Vector3(
            eventData.pointerEnter.transform.position.x,
            eventData.pointerEnter.transform.position.y,
            zoomedInViewCamera.transform.position.z
        );
    }
}
