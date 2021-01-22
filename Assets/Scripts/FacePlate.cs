using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlate : MonoBehaviour
{
    [SerializeField]
    bool isOpened;

    [SerializeField]
    int curExpressionIndex = 0;

    [SerializeField]
    List<GameObject> expressions = new List<GameObject>();

    [SerializeField]
    List<GameObject> brokenExpressions = new List<GameObject>();

    /// <summary>
    /// Store random "broken expressions"
    /// </summary>
    List<int> brokenExpressionIndexes = new List<int>();
    
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        ChooseRandomBrokenExpressions();
    }

    private void Update()
    {
        animator.SetBool("IsOpened", isOpened);
        if (Input.GetKeyDown(KeyCode.Space))
            ChooseRandomBrokenExpressions();
    }

    private void ChooseRandomBrokenExpressions()
    {
        brokenExpressionIndexes.Clear();

        var min = 1;
        var max = brokenExpressions.Count;

        for (int i = 0; i < Random.Range(0, max); i++)
        {
            int randIndex = 0;

            do
            {
                randIndex = Random.Range(min, max);
            } while (brokenExpressionIndexes.Contains(randIndex));

            brokenExpressionIndexes.Add(randIndex);
        }
    }

    public void ToggleFacePlateState()
    {
        isOpened = !isOpened;
    }

    public void NextExpression()
    {
        // Turn current one OFF
        SetExpressionState(false);

        // Choose next one
        curExpressionIndex++;
        if (curExpressionIndex > expressions.Count - 1)
            curExpressionIndex = 0;

        // Turn new current one ON
        SetExpressionState(true);
    }

    private void SetExpressionState(bool newState)
    {
        if (brokenExpressionIndexes.Contains(curExpressionIndex))
            brokenExpressions[curExpressionIndex].SetActive(newState);
        else
            expressions[curExpressionIndex].SetActive(newState);
    }
}
