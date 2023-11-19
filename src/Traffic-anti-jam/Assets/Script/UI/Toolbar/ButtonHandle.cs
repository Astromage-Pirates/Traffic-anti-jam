using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandle : MonoBehaviour
{
    [SerializeField]
    private Button button;

    private IEventBus eventBus;

    private void Awake()
    {
        GlobalServiceContainer.Resolve<IEventBus>(out eventBus);
        eventBus.Register<SnapPointViewed>(OnBtnToolBarClick);

    }

    private void OnBtnToolBarClick(SnapPointViewed snapPointViewed)
    {
        button.enabled = snapPointViewed.isToolBarBtnActive;
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<SnapPointViewed>(OnBtnToolBarClick);
    }
}
