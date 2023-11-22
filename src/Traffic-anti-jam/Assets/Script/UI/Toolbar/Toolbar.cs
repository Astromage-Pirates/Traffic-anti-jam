using System;
using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using JetBrains.Annotations;
using UnityEditor.Callbacks;
using UnityEngine;

public class Toolbar : MonoBehaviour
{
    private float targetX;
    private float sideStep = -160;
    private float sideSpeed = 5f;

    [SerializeField]
    private Transform sideBtn;

    [SerializeField]
    private TrafficTool trafficTool;

    [SerializeField]
    private Camera myCamera;

    [SerializeField]
    private Transform grp_TrafficTool;

    private IEventBus OnTrafficToolGenerated;

    private TrafficTool newTrafficTool;

    public List<TrafficTool> TrafficTools = new();

    private void Awake()
    {
        targetX = transform.position.x;
        GlobalServiceContainer.Resolve<IEventBus>(out OnTrafficToolGenerated);
        OnTrafficToolGenerated.Register<TrafficToolGenerated>(OnAddTraffictoolToList);
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

    public void CreateTrafficTool()
    {
        if (OnTrafficToolGenerated is not null)
        {
            newTrafficTool = Instantiate(trafficTool, grp_TrafficTool);
            OnTrafficToolGenerated.Send(new TrafficToolGenerated() { isSnapPointActive = true, isToolBarBtnActive = false });
        }
    }

    public void DeleteTrafficTool()
    {
        if (!newTrafficTool.isSnaped)
        {
            Destroy(newTrafficTool.gameObject);
            OnTrafficToolGenerated.Send(new TrafficToolGenerated() { isSnapPointActive = false, isToolBarBtnActive = true });
        }
    }

    public void OnAddTraffictoolToList(TrafficToolGenerated trafficToolGenerated)
    {
        if (newTrafficTool.isSnaped)
        {
            TrafficTools.Add(newTrafficTool);
        }
    }

    public void DeleteAllTrafficTool()
    {
        foreach (TrafficTool trafficTool in TrafficTools)
        {
            if (trafficTool)
            {
                Destroy(trafficTool.gameObject);
            }
        }

        TrafficTools.Clear();
    }

    private void OnDestroy()
    {
        OnTrafficToolGenerated.UnRegister<TrafficToolGenerated>(OnAddTraffictoolToList);
    }
}