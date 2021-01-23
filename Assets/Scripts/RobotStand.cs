using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStand : MonoBehaviour
{
    [SerializeField]
    float rotationAngle = 180;

    CameraController cameraController;

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    public void Rotate(int dir)
    {
        if (!cameraController.IsZoomedOut)
            return;

        dir = Mathf.Clamp(dir, -1, 1);
        transform.rotation = transform.rotation * Quaternion.Euler(0f, rotationAngle * dir, 0f);
    }
}
