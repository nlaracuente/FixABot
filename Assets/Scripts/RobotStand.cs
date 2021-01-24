using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStand : MonoBehaviour
{
    [SerializeField]
    float rotationAngle = 180;

    [SerializeField]
    float rotationTime = 1f;

    bool isRotating;

    public void Rotate(int dir)
    {
        dir = Mathf.Clamp(dir, -1, 1);
        if(!isRotating)
            StartCoroutine(Rotate(Vector3.up, dir * rotationAngle, rotationTime));
    }

    IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)
    {
        isRotating = true;

        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = to;
        isRotating = false;
    }
}
