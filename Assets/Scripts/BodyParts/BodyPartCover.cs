﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class BodyPartCover : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    BodyPart bodyPart;

    [SerializeField]
    GameObject fixedCover;

    [SerializeField]
    GameObject brokenCover;

    new Collider collider;

    bool isFixing = false;


    private void Start()
    {
        collider = GetComponent<Collider>();
        UpdateCovers();
    }

    /// <summary>
    /// Waits until the body is fixed to put back the cover of being fixed
    /// </summary>
    void Update()
    {
        if (!isFixing)
            return;

        if (isFixing && bodyPart.IsFixed)
            UpdateCovers();
    }

    void UpdateCovers()
    {
        fixedCover?.SetActive(bodyPart.IsFixed);
        brokenCover?.SetActive(!bodyPart.IsFixed);

        if (fixedCover.activeSelf)
            collider.enabled = false;
    }

    /// <summary>
    /// Remove covering and collider to avoid being able to click again
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        isFixing = true;
        collider.enabled = false;
        brokenCover?.SetActive(false);
    }
}