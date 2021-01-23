using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCover : MonoBehaviour
{
    [SerializeField]
    bool isOpened;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("IsOpened", isOpened);
    }

    public void ToggleCoverState()
    {
        isOpened = !isOpened;
    }
}
