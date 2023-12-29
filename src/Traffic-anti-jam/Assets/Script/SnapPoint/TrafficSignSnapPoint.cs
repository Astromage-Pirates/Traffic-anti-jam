using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AstroPirate.DesignPatterns;
using UnityEngine;

public class TrafficSignSnapPoint : SnapPoint<TrafficSign>
{
    [SerializeField]
    private Path[] forwardPaths;

    [SerializeField]
    private Path[] leftPaths;

    [SerializeField]
    private Path[] rightPaths;

    protected override void Awake()
    {
        base.Awake();
        eventBus.Register<TrafficSignUIInteracted>(OnTrafficSignSnapPointActive);
        OnTrafficSignSnapPointActive(
            new TrafficSignUIInteracted() { isSnapPointActive = false, isToolBarBtnActive = true }
        );
    }

    private void OnTrafficSignSnapPointActive(TrafficSignUIInteracted trafficSignUIInteracted)
    {
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

        TrafficSignEffect(currTrafficTool);
    }

    private void TrafficSignEffect(TrafficSign trafficSign)
    {
        switch (trafficSign)
        {
            case ForwardSign _:
                SetPath(false, leftPaths, rightPaths);
                SetPath(true, forwardPaths);
                break;

            case LeftSign _:
                SetPath(false, forwardPaths, rightPaths);
                SetPath(true, leftPaths);
                break;

            case RightSign _:
                SetPath(false, forwardPaths, leftPaths);
                SetPath(true, rightPaths);
                break;

            case NoLeftSign _:
                SetPath(false, leftPaths);
                SetPath(true, rightPaths, forwardPaths);
                break;

            case NoRightSign _:
                SetPath(false, rightPaths);
                SetPath(true, leftPaths, forwardPaths);
                break;

            case OneWaySign _:
                SetPath(false, leftPaths, rightPaths, forwardPaths);
                break;
        }
    }

    private void SetPath(bool available, params Path[][] arrPaths)
    {
        foreach (Path[] paths in arrPaths)
        {
            foreach (Path path in paths)
            {
                if (path)
                {
                    path.Available = available;
                }
            }
        }
    }

    public void UnDoPath(params Path[][] arrPaths)
    {
        if (arrPaths.Length == 0)
        {
            UnDoPath(leftPaths, rightPaths, forwardPaths);
            return;
        }

        foreach (Path[] paths in arrPaths)
        {
            foreach (Path path in paths)
            {
                if (path)
                {
                    path.Available = path.InitAvailable;
                }
            }
        }
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<TrafficSignUIInteracted>(OnTrafficSignSnapPointActive);
    }
}
