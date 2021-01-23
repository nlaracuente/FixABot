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
    public bool IsSolved { get; protected set; }
    public abstract void OnPointerClick(PointerEventData eventData);
    public abstract void OnPointerEnter(PointerEventData eventData);
    public abstract void OnPointerExit(PointerEventData eventData);
}
