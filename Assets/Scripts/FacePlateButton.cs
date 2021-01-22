using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FacePlateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    bool isMouseOver;

    Animator animator;
    FacePlate facePlate;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        facePlate = FindObjectOfType<FacePlate>();
    }

    private void Update()
    {
        animator.SetBool("IsMouseOver", isMouseOver);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        facePlate.ToggleFacePlateState();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    

}
