using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandle : MonoBehaviour
{
    [SerializeField]
    private Button button;

    private IEventBus EventBus;

    private void Awake()
    {
        GlobalServiceContainer.Resolve<IEventBus>(out EventBus);
        EventBus.Register<TrafficSignUIInteracted>(OnBtnToolBarClick);
        EventBus.Register<TrafficLightUIInteracted>(OnBtnToolBarClick);
    }

    private void OnBtnToolBarClick(TrafficToolUIInteracted trafficToolUIInteracted)
    {
        if (trafficToolUIInteracted is TrafficSignUIInteracted trafficSignUIInteracted)
        {
            button.enabled = trafficSignUIInteracted.isToolBarBtnActive;
        }

        if (trafficToolUIInteracted is TrafficLightUIInteracted trafficLightUIInteracted)
        {
            button.enabled = trafficLightUIInteracted.isToolBarBtnActive;
        }
    }

    private void OnDestroy()
    {
        EventBus.UnRegister<TrafficSignUIInteracted>(OnBtnToolBarClick);
        EventBus.UnRegister<TrafficLightUIInteracted>(OnBtnToolBarClick);
    }
}
