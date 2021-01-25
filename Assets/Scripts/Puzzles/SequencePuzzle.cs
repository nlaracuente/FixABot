using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class SequencePuzzle : Puzzle
{
    [SerializeField]
    Color defaultColor = Color.black;

    [SerializeField]
    Color sequenceColor = Color.yellow;

    [SerializeField]
    Color invalidColor = Color.red;

    [SerializeField]
    float sequenceDelay = .25f;

    [SerializeField, Tooltip("How long to wait before checking player's input")]
    float prevalidationDelay = .10f;

    [SerializeField]
    float invalidDelay = .10f;

    [SerializeField]
    int totalErrorBlinks = 3;

    [SerializeField]
    Renderer[] buttons;

    MaterialPropertyBlock propBlock;

    Renderer[] sequence;
    List<Renderer> buttonsClicked;

    IEnumerator currentRoutine;

    bool isSolved = false;
    public override bool IsSolved { get { return isSolved; } }

    public override void BuildPuzzle()
    {
        propBlock = new MaterialPropertyBlock();
        sequence = Utility.ShuffleArray(buttons, RandomNumbers.Seed);
        buttonsClicked = new List<Renderer>();
    }

    public override void ShowPuzzle()
    {
        base.ShowPuzzle();
        PlaySequence();
    }

    void PlaySequence()
    {
        if (currentRoutine != null)
            return;

        sequence = Utility.ShuffleArray(buttons, RandomNumbers.Seed);
        ResetButtonColors();
        currentRoutine = SequenceRoutine();
        StartCoroutine(currentRoutine);
    }

    void ResetButtonColors() => SetColorForAllButtons(defaultColor);
    void SetColorForAllButtons(Color color)
    {
        foreach (var button in sequence)
            SetRendererColor(button, color);
    }

    void SetRendererColor(Renderer renderer, Color color)
    {
        renderer.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", color);
        renderer.SetPropertyBlock(propBlock);
    }

    IEnumerator SequenceRoutine()
    {
        yield return new WaitForSeconds(sequenceDelay);
        foreach (var button in sequence)
        {
            SetRendererColor(button, sequenceColor);
            yield return new WaitForSeconds(sequenceDelay);
            SetRendererColor(button, defaultColor);
        }

        currentRoutine = null;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        // Ignore clicks while a routine is running
        if (currentRoutine != null)
            return;

        // Ignore already clicked on
        var renderer = eventData.pointerEnter.GetComponent<Renderer>();
        if (buttonsClicked.Contains(renderer))
            return;

        AudioManager.instance.ButtonClicked();
        SetRendererColor(renderer, sequenceColor);

        currentRoutine = ValidateSequenceRoutine(renderer);
        StartCoroutine(currentRoutine);
    }

    IEnumerator ValidateSequenceRoutine(Renderer renderer)
    {
        yield return new WaitForSeconds(prevalidationDelay);

        var index = buttonsClicked.Count;
        var expected = sequence[index];
        if (renderer == expected)
            buttonsClicked.Add(renderer);
        else
        {
            // Display error and replay sequence
            buttonsClicked.Clear();
            yield return StartCoroutine(InvalidSequenceRoutine());
            yield return StartCoroutine(SequenceRoutine());
        }

        // All buttons clicked in the expected order
        isSolved = buttonsClicked.Count() == sequence.Count();
        currentRoutine = null;
    }

    IEnumerator InvalidSequenceRoutine()
    {
        for (int i = 0; i < totalErrorBlinks; i++)
        {
            SetColorForAllButtons(invalidColor);
            yield return new WaitForSeconds(invalidDelay);
            ResetButtonColors();
            yield return new WaitForSeconds(invalidDelay);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
