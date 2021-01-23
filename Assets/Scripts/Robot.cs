using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public void Initialize(int totalBrokenParts)
    {
        Debug.Log($"{name}: total broken parts: {totalBrokenParts}");
    }
}
