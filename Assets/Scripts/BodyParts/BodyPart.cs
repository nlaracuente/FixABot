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
    public bool IsFixed { get { return puzzle == null || puzzle.IsSolved; } }

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

        // Shuffle the puzzles to randomize them
        puzzles = new List<Puzzle>(Utility.ShuffleArray(puzzles.ToArray(), RandomNumbers.Seed));        
        var randPuzzle = puzzles[0];

        // TODO: allow the puzzles to auto fix the body part based on its rotation/position
        var go = Instantiate(randPuzzle, transform);
        puzzle = go.GetComponent<Puzzle>();
    }

    public void ShowPuzzle()
    {
        puzzle?.ShowPuzzle();
    }

    public void HidePuzzle()
    {
        puzzle?.HidePuzzle();
    }

    public virtual void OnPointerClick(PointerEventData eventData) { }

    public virtual void OnPointerEnter(PointerEventData eventData) { }

    public virtual void OnPointerExit(PointerEventData eventData) { }
}
