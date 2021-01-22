using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FacePlate : MonoBehaviour
{
    public enum Expression
    {
        None,
        Happy,
        Sad,
        Angry,
    }

    [SerializeField]
    bool isOpened;

    [SerializeField]
    int curExpressionIndex = 0;

    [SerializeField]
    List<GameObject> expressions = new List<GameObject>();

    [SerializeField]
    List<GameObject> brokenExpressionsPrefabs = new List<GameObject>();

    [SerializeField]
    FixButton fixButton;

    /// <summary>
    /// Store random "broken expressions"
    /// </summary>
    List<Expression> brokenExpression = new List<Expression>();
    public List<Expression> BrokenExpressions { get { return brokenExpression; } }
    
    Animator animator;

    Expression CurrentExpression { get { return (Expression)curExpressionIndex; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        ChooseRandomBrokenExpressions();
    }

    private void Start()
    {
        if (fixButton == null)
            fixButton = GetComponentInChildren<FixButton>();

        fixButton.RegisterEvent(FixCurrentExpression);
    }

    private void Update()
    {
        animator.SetBool("IsOpened", isOpened);
        if (Input.GetKeyDown(KeyCode.Space))
            ChooseRandomBrokenExpressions();
    }

    private void ChooseRandomBrokenExpressions()
    {
        brokenExpression.Clear();

        var min = 1;
        var max = brokenExpressionsPrefabs.Count;

        for (int i = 0; i < UnityEngine.Random.Range(0, max); i++)
        {
            int rIdx;
            Expression exp;

            do
            {
                rIdx = UnityEngine.Random.Range(min, max);
                exp = (Expression)rIdx;
            } while (brokenExpression.Contains(exp));

            brokenExpression.Add(exp);
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
        if (brokenExpression.Contains(CurrentExpression))
            brokenExpressionsPrefabs[curExpressionIndex].SetActive(newState);
        else
            expressions[curExpressionIndex].SetActive(newState);
    }

    public void FixCurrentExpression()
    {
        // Turn off the broken one first before fixing it
        if (brokenExpression.Contains(CurrentExpression))
        {
            SetExpressionState(false);
            brokenExpression.Remove(CurrentExpression);
        }

        SetExpressionState(true);

        // Unregister if everything is fixed
        if (brokenExpression.Count < 1)
            fixButton.UnregisterEvent(FixCurrentExpression);
    }

    public bool IsExpressionBroken(Expression expression)
    {
        return brokenExpression.Contains(expression);
    }
}
