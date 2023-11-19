using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficTool : MonoBehaviour
{
    private Camera camera;

    private LayerMask layer;

    public bool isSnaped = false;

    [SerializeField]
    public GameObject greenDisc;

    [SerializeField]
    public GameObject redDisc;

    private void Start()
    {
        camera = Camera.main;
        layer = LayerMask.GetMask("Pavement");
    }

    private void FixedUpdate()
    {
        FollowingMouse();

    }

    public void FollowingMouse()
    {
        if (!isSnaped)
        {
            Vector3 mousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane + 10));
            if (Physics.Raycast(mousePos, mousePos - camera.transform.position, out var hit, 100f, layer))
            {
                mousePos = hit.point;
            }
            transform.position = mousePos;
            Debug.DrawRay(mousePos, (mousePos - camera.transform.position) * 100f, Color.red);
        }
    }
}
