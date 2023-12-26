using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using UnityEngine;

public abstract class SnapPoint<T> : DirectionPoint
    where T : TrafficTool
{
    [SerializeField]
    protected MeshRenderer myMeshRenderer;

    [SerializeField]
    protected Collider myCollider;

    protected T currTrafficTool;

    protected IEventBus eventBus;

    protected virtual void Awake()
    {
        GlobalServiceContainer.Resolve<IEventBus>(out eventBus);
    }

    protected void OnTriggerStay(Collider other)
    {
        CheckTrafficToolSnapIn(other);
    }

    protected abstract void OnSnap(T trafficTool);

    protected void CheckTrafficToolSnapIn(Collider other)
    {
        if (other.transform.TryGetComponent<T>(out T newTrafficTool))
        {
            if (newTrafficTool.isSnaped)
                return;

            newTrafficTool.DiscColor(newTrafficTool.greenDisc);

            if (Input.GetMouseButton(0))
            {
                if (currTrafficTool)
                {
                    eventBus.Send(new BudgetCost() { trafficTool = currTrafficTool, intSign = -1 });
                    Destroy(currTrafficTool.gameObject);
                }

                currTrafficTool = newTrafficTool;
                OnSnap(currTrafficTool);
            }
        }
    }

    protected void CheckTrafficToolSnapOut(Collider other)
    {
        if (other.transform.TryGetComponent<T>(out T newTrafficTool))
        {
            newTrafficTool.DiscColor(newTrafficTool.redDisc);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        CheckTrafficToolSnapOut(other);
    }
}
