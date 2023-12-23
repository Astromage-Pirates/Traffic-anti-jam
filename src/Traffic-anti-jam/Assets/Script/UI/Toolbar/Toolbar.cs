using System;
using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using UnityEngine;

public class Toolbar : MonoBehaviour
{
    private float targetX;
    private float sideStep = -160;
    private float sideSpeed = 5f;

    [SerializeField]
    private Transform sideBtn;

    [SerializeField]
    private TrafficSign trafficSign_prefab;

    [SerializeField]
    private TrafficLight trafficLight_prefab;

    [SerializeField]
    private Transform grp_TrafficTool;

    private IEventBus eventBus;

    private TrafficTool newTrafficTool;

    [SerializeField]
    private List<TrafficTool> TrafficTools = new();

    private void Awake()
    {
        targetX = transform.position.x;
        GlobalServiceContainer.Resolve<IEventBus>(out eventBus);
        eventBus.Register<TrafficToolUIInteracted>(OnAddTraffictoolToList);
    }

    private void FixedUpdate()
    {
        ToolbarHandle();
    }

    public void ShowToolbar()
    {
        float rotateZ = 180;
        targetX = transform.position.x + sideStep;
        sideBtn.Rotate(0, 0, rotateZ);
        sideStep *= -1;
    }

    private void ToolbarHandle()
    {
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, targetX, sideSpeed * Time.deltaTime),
            transform.position.y,
            0.0f
        );
    }

    public void CreateTrafficSign()
    {
        if (eventBus is not null)
        {
            newTrafficTool = Instantiate(trafficSign_prefab, grp_TrafficTool);
            eventBus.Send(
                new TrafficSignUIInteracted()
                {
                    isSnapPointActive = true,
                    isToolBarBtnActive = false
                }
            );
        }
    }

    public void CreateTrafficLight()
    {
        if (eventBus is not null)
        {
            newTrafficTool = Instantiate(trafficLight_prefab, grp_TrafficTool);
            eventBus.Send(
                new TrafficLightUIInteracted()
                {
                    isSnapPointActive = true,
                    isToolBarBtnActive = false
                }
            );
        }
    }

    public void DeleteTrafficTool()
    {
        if (newTrafficTool)
        {
            if (!newTrafficTool.isSnaped)
            {
                Destroy(newTrafficTool.gameObject);
                eventBus.Send(
                    new TrafficSignUIInteracted()
                    {
                        isSnapPointActive = false,
                        isToolBarBtnActive = true
                    }
                );

                eventBus.Send(
                    new TrafficLightUIInteracted()
                    {
                        isSnapPointActive = false,
                        isToolBarBtnActive = true
                    }
                );
            }
        }
    }

    public void OnAddTraffictoolToList(TrafficToolUIInteracted trafficToolUIInteracted)
    {
        if (newTrafficTool.isSnaped)
        {
            TrafficTools.Add(newTrafficTool);
            newTrafficTool = null;
        }
    }

    public void DeleteAllTrafficTool()
    {
        foreach (TrafficTool trafficTool in TrafficTools)
        {
            if (trafficTool)
            {
                eventBus.Send(new BudgetCost() { trafficTool = trafficTool, intSign = -1 });
                Destroy(trafficTool.gameObject);
            }
        }

        TrafficTools.Clear();
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<TrafficSignUIInteracted>(OnAddTraffictoolToList);
    }
}
