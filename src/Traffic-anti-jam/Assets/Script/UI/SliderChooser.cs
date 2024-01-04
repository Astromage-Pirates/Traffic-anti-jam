using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Choose an object from a slider list.
/// </summary>
public class SliderChooser : MonoBehaviour
{
    [SerializeField]
    private GameObject[] selectionList;

    [SerializeField]
    private Scrollbar scrollbar;

    [SerializeField]
    private float scrollDuration = 0.15f;

    [Header("Controls")]
    [SerializeField]
    private Button btn_Back;

    [SerializeField]
    private Button btn_Next;

    private float[] objectPositions;
    private float distance = 0;
    private float slide_position = 0;
    private int currentPosition = 0;

    private void Start()
    {
        InitializeValue();
    }

    protected virtual void OnEnable()
    {
        btn_Next.onClick.AddListener(SlideNext);
        btn_Back.onClick.AddListener(SlidePrev);
    }

    protected virtual void OnDisable()
    {
        btn_Next.onClick.RemoveListener(SlideNext);
        btn_Back.onClick.RemoveListener(SlidePrev);
    }

    private void Update()
    {
        for (int i = 0; i < objectPositions.Length; i++)
        {
            var objectPosition = objectPositions[i];
            var halfDistance = distance / 2;

            if (slide_position < objectPosition + halfDistance && slide_position > objectPosition - halfDistance)
            {
                DOTween.To(() => scrollbar.value, value => scrollbar.value = value, objectPositions[i], scrollDuration);
            }
        }
    }

    private void SlideNext()
    {
        if (currentPosition < objectPositions.Length - 1)
        {
            currentPosition += 1;
            slide_position = objectPositions[currentPosition];
        }

        SetButtonsInteractableState();
    }

    private void SlidePrev()
    {
        if (currentPosition > 0)
        {
            currentPosition -= 1;
            slide_position = objectPositions[currentPosition];
        }

        SetButtonsInteractableState();
    }

    private void InitializeValue()
    {
        objectPositions = new float[selectionList.Length];
        distance = 1f / (objectPositions.Length - 1);

        for (int i = 0; i < objectPositions.Length; i++)
        {
            objectPositions[i] = distance * i;
        }

        SetButtonsInteractableState();
    }

    private void SetButtonsInteractableState()
    {
        btn_Back.interactable = currentPosition > 0;
        btn_Next.interactable = currentPosition < objectPositions.Length - 1;
    }
}
