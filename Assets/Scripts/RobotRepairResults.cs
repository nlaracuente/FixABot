using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotRepairResults : MonoBehaviour
{
    [SerializeField]
    Text results;

    public void SetResults(bool isFixed)
    {
        results.text = isFixed? "Repaired!": "Not Repaired :(";
    }
}
