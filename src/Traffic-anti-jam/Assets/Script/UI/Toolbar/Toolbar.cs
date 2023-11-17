using System;
using System.Collections;
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
    private TrafficTool trafficTool;

    [SerializeField]
    private Camera camera;

    private IEventBus eventBus;

    private void Awake()
    {
        targetX = transform.position.x;
        GlobalServiceContainer.Resolve<IEventBus>(out eventBus);
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

    public void DragTrafficTool()
    {
        if (eventBus is not null)
        {
            eventBus.Send(new SnapPointViewed() { isActive = true });
            var newObj = Instantiate(trafficTool);
        }
    }
}
