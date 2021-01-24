using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Robot : MonoBehaviour
{
    [SerializeField, Tooltip("Body parts that can be broken")]
    BodyPart[] breakableParts;

    /// <summary>
    /// All breakable parts are not broken
    /// </summary>
    public bool IsRepaired { get { return breakableParts.Where(b => !b.IsFixed).FirstOrDefault() == null; } }

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