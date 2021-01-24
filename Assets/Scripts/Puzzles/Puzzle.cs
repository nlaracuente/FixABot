using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// All puzzles are essentially "Click" to solve since this is single click game
/// I could make this into an interface...but for now this is fine
/// </summary>
public abstract class Puzzle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    protected bool isBuilt;
    public virtual bool IsSolved { get; }

    private void Start()
    {
        BuildPuzzle();
        HidePuzzle();
        isBuilt = true;
    }

    private void Update()
    {
        if (IsSolved)
            HidePuzzle();
    }

    public void HidePuzzle()
    {
        gameObject.SetActive(false);
    }

    public void ShowPuzzle()
    {
        gameObject.SetActive(true);
    }

    public abstract void BuildPuzzle();
    public abstract void OnPointerClick(PointerEventData eventData);
    public abstract void OnPointerEnter(PointerEventData eventData);
    public abstract void OnPointerExit(PointerEventData eventData);
}
