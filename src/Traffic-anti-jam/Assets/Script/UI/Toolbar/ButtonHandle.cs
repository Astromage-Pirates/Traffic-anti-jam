using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandle : MonoBehaviour
{
    [SerializeField]
    private Button button;

    private IEventBus OnTrafficToolGenerated;

    private void Awake()
    {
        GlobalServiceContainer.Resolve<IEventBus>(out OnTrafficToolGenerated);
        OnTrafficToolGenerated.Register<TrafficToolGenerated>(OnBtnToolBarClick);

    }

    private void OnBtnToolBarClick(TrafficToolGenerated snapPointViewed)
    {
        button.enabled = snapPointViewed.isToolBarBtnActive;
    }

    private void OnDestroy()
    {
        OnTrafficToolGenerated.UnRegister<TrafficToolGenerated>(OnBtnToolBarClick);
    }
}
