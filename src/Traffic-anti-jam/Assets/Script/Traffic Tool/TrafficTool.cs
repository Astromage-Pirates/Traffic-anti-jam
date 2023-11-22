using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrafficTool : MonoBehaviour, IPointerClickHandler
{
    private Camera camera;

    private LayerMask layer;

    public bool isSnaped = false;

    private int isShowedCheck = 1;

    [SerializeField]
    public GameObject greenDisc;

    [SerializeField]
    public GameObject redDisc;

    [SerializeField]
    private GameObject delBtn;

    [SerializeField]
    private Outline outline;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSnaped)
        {
            bool isShowed = isShowedCheck > 0;
            isShowedCheck *= -1;
            ShowUI(isShowed);
        }
    }

    public void DestroyTrafficTool()
    {
        Destroy(this.gameObject);
    }

    public void ShowUI(bool isShowed)
    {
        delBtn.SetActive(isShowed);
        outline.enabled = isShowed;
    }

    private void OnShowUIGameStarted()
    {
        ShowUI(false);
    }

}
