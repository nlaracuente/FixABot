using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeekMenuController : MonoBehaviour
{
    [SerializeField]
    Text dayText;

    [SerializeField]
    Text robotText;

    [SerializeField]
    Text repairedText;

    public void SetDayText(int current, int total) 
    {
        //dayText.text = $"Day: {current} / {total}";
    }

    public void SetRobotText(int current, int total)
    {
        robotText.text = $"Robot: {current} / {total}";
    }

    public void SetRepairedText(int current, int total)
    {
        repairedText.text = $"Reapired: {current} / {total}";
    }
}
