using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField, Tooltip("Body parts that can be broken")]
    BodyPart[] breakableParts;

    public void Initialize(int totalBrokenParts)
    {
        if(breakableParts == null || breakableParts.Length < 1)
        {
            Debug.Log($"{name}: has no assigned breakable body parts");
            return;
        }

        totalBrokenParts = Mathf.Clamp(totalBrokenParts, 0, breakableParts.Length);

        var bodyParts = Utility.ShuffleArray(breakableParts, RGN.Between(1, 1000000));
        for (int i = 0; i < totalBrokenParts; i++)
            bodyParts[i].Break();
    }
}