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
    
    [SerializeField]
    private PathNode Intersection;

	[field: SerializeField]
    public PathNode forwardEdge { get; private set; }
    [field: SerializeField]
    public PathNode rightEdge { get; private set; }
    [field: SerializeField]
    public PathNode leftEdge { get; private set; }

    [SerializeField]
    private List<BetterPath> betterPath;

    protected override void Awake()
    {
        base.Awake();
        eventBus.Register<TrafficSignUIInteracted>(OnTrafficSignSnapPointActive);
        OnTrafficSignSnapPointActive(
            new TrafficSignUIInteracted() { isSnapPointActive = false, isToolBarBtnActive = true }
        );
    }

	private void Start()
	{

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
        trafficSign.OnSnap();
        TrafficSignEffect(currTrafficTool);
    }

    private void TrafficSignEffect(TrafficSign trafficSign)
    {
        switch (trafficSign)
        {
            case ForwardSign _:
                SetNode(false, leftEdge, rightEdge);
                SetNode(true, forwardEdge);

                SetPath(false, leftPaths, rightPaths);
                SetPath(true, forwardPaths);
                break;

            case LeftSign _:
				SetNode(false, forwardEdge, rightEdge);
				SetNode(true, leftEdge);
                
                SetPath(false, forwardPaths, rightPaths);
                SetPath(true, leftPaths);
                break;

            case RightSign _:
				SetNode(false, forwardEdge, leftEdge);
				SetNode(true, rightEdge);
                
                SetPath(false, forwardPaths, leftPaths);
                SetPath(true, rightPaths);
                break;

            case NoLeftSign _:
				SetNode(false, leftEdge);
				SetNode(true, rightEdge, forwardEdge);
                
                SetPath(false, leftPaths);
                SetPath(true, rightPaths, forwardPaths);
                break;

            case NoRightSign _:
				SetNode(false, rightEdge);
				SetNode(true, leftEdge, forwardEdge);
                
                SetPath(false, rightPaths);
                SetPath(true, leftPaths, forwardPaths);
                break;

            case OneWaySign _:
				SetNode(false, leftEdge, rightEdge, forwardEdge);
                
                SetPath(false, leftPaths, rightPaths, forwardPaths);
                break;
        }
		foreach (var path in betterPath)
		{
			path.evaluateRoute();
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
    
    private void SetNode(bool available, params PathNode[] arrPaths)
    {
        foreach (PathNode path in arrPaths)
        {
            if(path)
            {
                foreach (var edge in Intersection.Edges)
                {
                    if(edge.To == path)
                    {
                        edge.Weight = available?0:99999;
                    }
                }
            }
        }
    }

    public void UnDoPath(params Path[][] arrPaths)
    {
        if (arrPaths.Length == 0)
        {
			foreach (var edge in Intersection.Edges)
			{
			    edge.Weight = 0;
			}
            foreach (var path in betterPath)
            {
                path.evaluateRoute();
            }
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
