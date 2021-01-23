using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumbers : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.G))
            Debug.Log("Rand 0-20: " + RGN.Between(1, 3));
    }
}
