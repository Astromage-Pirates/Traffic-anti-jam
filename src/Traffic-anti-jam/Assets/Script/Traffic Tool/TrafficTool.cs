using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficTool : MonoBehaviour
{
    private Camera camera;

    private LayerMask layer;

    private bool isSnaped = false;

    [SerializeField]
    private GameObject greenDisc;

    [SerializeField]
    private GameObject redDisc;

    private void Start()
    {
        camera = Camera.main;
        layer = LayerMask.GetMask("Pavement");
    }
    private void Update()
    {
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("0");
        }
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

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.TryGetComponent<SnapPoint>(out SnapPoint snapPoint))
        {
            redDisc.SetActive(false);
            greenDisc.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("0");
                isSnaped = true;
                greenDisc.SetActive(false);
                gameObject.transform.position = snapPoint.transform.position;
                gameObject.transform.Rotate(0, transform.rotation.y + (int)snapPoint.direction, 0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent<SnapPoint>(out SnapPoint snapPoint))
        {
            greenDisc.SetActive(false);
            redDisc.SetActive(true);
        }
    }
}
