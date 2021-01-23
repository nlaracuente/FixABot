﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BodyPart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField, Tooltip("Available puzzles to choose at random to fix this part")]
    protected List<Puzzle> puzzles = new List<Puzzle>();

    protected Puzzle puzzle;

    /// <summary>
    /// If a puzzle exist then it was marked as broken
    /// It remains broken until the puzzle is solved
    /// </summary>
    public bool IsBroken { get { return puzzle != null && !puzzle.IsSolved; } }

    /// <summary>
    /// Triggers this body part to break
    /// </summary>
    public void Break()
    {
        if (puzzles.Count < 1)
        {
            Debug.LogError($"{name} has not been given a list of puzzles. Cannot break it");
            return;
        }
        
        // TODO: Make this more random
        puzzle = puzzles[Random.Range(0, puzzles.Count)];
        Debug.Log($"{name}: marked as broken. Puzzle: {puzzle}");
    }

    /// <summary>
    /// Runs the sequence to see if the body part is broken
    /// </summary>
    public virtual void TestPart()
    {
        var status = IsBroken ? "is broken" : "is not broken";
        Debug.Log($"{name}: {status}");
    }

    public virtual void OnPointerClick(PointerEventData eventData) { }

    public virtual void OnPointerEnter(PointerEventData eventData) { }

    public virtual void OnPointerExit(PointerEventData eventData) { }
}
