using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsMenu : MonoBehaviour
{
    [SerializeField]
    Text resultsText;

    public void SetResults(int repaired, int total)
    {
        var score = repaired * 100;
        var percentage = (float)repaired / total;
        string results;

        if (percentage < 0.1f)
        {
            results = "Did you play at all?";
        }
        else if (percentage < 0.24f)
        {
            results = "You could've done better";
        }
        else if (percentage < 0.51f)
        {
            results = "Not bad...but you can do better.";
        }
        else if (percentage < 0.76f)
        {
            results = "Look at you! Nicely done";
        }
        else if (percentage < 1f)
        {
            results = "You did well! Almost perfect!";
        }
        else
        {
            results = "You did it! Christmas is saved!";
        }

        var text = $"You repaired {repaired} / {total} robots.\n" +
                   $"That gives you a total score of {score} points!\n\n" +
                   $"\"{results}\"\n\n" +
                   "Thanks for playing!";

        resultsText.text = text;
    }
}
