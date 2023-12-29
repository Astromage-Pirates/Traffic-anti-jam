using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSign : TrafficTool
{
    private TrafficSignSnapPoint trafficSignSnapPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TrafficSignSnapPoint>(out TrafficSignSnapPoint component))
        {
            trafficSignSnapPoint = component;
        }
    }

    private void OnDestroy()
    {
        trafficSignSnapPoint.UnDoPath();
    }
}
