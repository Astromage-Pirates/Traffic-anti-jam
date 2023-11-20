using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    public enum Direction
    {
        forward = 0,
        backward = 180,
        left = 90,
        right = -90
    }

    private TrafficTool newTrafficTool;

    private TrafficTool currTrafficTool;

    private IEventBus OnTrafficToolGenerated;

    [SerializeField]
    public Direction direction;

    [SerializeField]
    private MeshRenderer myMeshRenderer;

    [SerializeField]
    private Collider myCollider;

    private void Awake()
    {
        GlobalServiceContainer.Resolve<IEventBus>(out OnTrafficToolGenerated);
        OnTrafficToolGenerated.Register<TrafficToolGenerated>(OnSnapPointActive);
        OnTrafficToolGenerated.Send(new TrafficToolGenerated() { isSnapPointActive = false, isToolBarBtnActive = true });
    }

    private void OnSnapPointActive(TrafficToolGenerated trafficToolGenerated)
    {
        myCollider.enabled = trafficToolGenerated.isSnapPointActive;
        myMeshRenderer.enabled = trafficToolGenerated.isSnapPointActive;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.TryGetComponent<TrafficTool>(out newTrafficTool))
        {
            if (newTrafficTool.isSnaped)
                return;

            newTrafficTool.greenDisc.SetActive(true);
            newTrafficTool.redDisc.SetActive(false);

            if (Input.GetMouseButton(0))
            {
                if (currTrafficTool)
                {
                    Destroy(currTrafficTool.gameObject);

                }

                currTrafficTool = newTrafficTool;
                currTrafficTool.isSnaped = true;
                TrafficToolLocated(currTrafficTool);
                OnTrafficToolGenerated.Send(new TrafficToolGenerated() { isSnapPointActive = false, isToolBarBtnActive = true });
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent<TrafficTool>(out newTrafficTool))
        {
            newTrafficTool.greenDisc.SetActive(false);
            newTrafficTool.redDisc.SetActive(true);
        }
    }

    private void TrafficToolLocated(TrafficTool trafficTool)
    {
        trafficTool.greenDisc.SetActive(false);
        trafficTool.transform.position = transform.position;
        trafficTool.transform.Rotate(0, trafficTool.transform.rotation.y + (int)direction, 0);
    }


    private void OnDestroy()
    {
        OnTrafficToolGenerated.UnRegister<TrafficToolGenerated>(OnSnapPointActive);
    }
}