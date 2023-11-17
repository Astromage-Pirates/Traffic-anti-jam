using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    public enum Direction
    {
        forward = 0,
        backward = 180,
        left = 90,
        right = -90
    }

    private IEventBus eventBus;

    [SerializeField]
    public Direction direction;

    private void OnSnapPointActive(SnapPointViewed snapPointViewed)
    {
        gameObject.SetActive(snapPointViewed.isActive);
    }

    private void Awake()
    {
        GlobalServiceContainer.Resolve<IEventBus>(out eventBus);
        eventBus.Register<SnapPointViewed>(OnSnapPointActive);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<SnapPointViewed>(OnSnapPointActive);
    }
}


