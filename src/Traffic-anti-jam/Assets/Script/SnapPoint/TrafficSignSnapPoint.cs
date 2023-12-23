using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using UnityEngine;

public class TrafficSignSnapPoint : SnapPoint<TrafficSign>
{
    private IEventBus eventBus;

    private void Awake()
    {
        GlobalServiceContainer.Resolve<IEventBus>(out eventBus);
        eventBus.Register<TrafficSignUIInteracted>(OnTrafficSignSnapPointActive);
        OnTrafficSignSnapPointActive(
            new TrafficSignUIInteracted() { isSnapPointActive = false, isToolBarBtnActive = true }
        );
    }

    private void OnTrafficSignSnapPointActive(TrafficSignUIInteracted trafficSignUIInteracted)
    {
        myCollider.enabled = trafficSignUIInteracted.isSnapPointActive;
        myMeshRenderer.enabled = trafficSignUIInteracted.isSnapPointActive;
    }

    protected override void OnSnap(TrafficSign trafficSign)
    {
        trafficSign.isSnaped = true;
        eventBus.Send(new TrafficToolUIInteracted());
        eventBus.Send(new BudgetCost() { trafficTool = trafficSign, intSign = 1 });
        trafficSign.DiscColor(null);
        trafficSign.transform.position = transform.position;
        trafficSign.transform.Rotate(0, trafficSign.transform.rotation.y + (int)direction, 0);
        eventBus.Send(
            new TrafficSignUIInteracted() { isSnapPointActive = false, isToolBarBtnActive = true }
        );
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<TrafficSignUIInteracted>(OnTrafficSignSnapPointActive);
    }
}
