using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

abstract public class TrafficTool : MonoBehaviour, IPointerClickHandler
{
    private Camera myCamera;

    private LayerMask layer;

    public bool isSnaped = false;

    private int isShowedCheck = 1;

    [SerializeField]
    public List<Material> greenDisc;

    [SerializeField]
    public List<Material> redDisc;

    [SerializeField]
    public GameObject disc;

    [SerializeField]
    protected Canvas traficToolCanvas;

    [SerializeField]
    private Outline outline;

    [SerializeField]
    public int cost;

    protected IEventBus eventBus;

    protected virtual void Awake()
    {
        GlobalServiceContainer.Resolve<IEventBus>(out eventBus);
    }

    private void Start()
    {
        myCamera = Camera.main;
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
            Vector3 mousePos = myCamera.ScreenToWorldPoint(
                new Vector3(
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    myCamera.nearClipPlane + 10
                )
            );
            if (
                Physics.Raycast(
                    mousePos,
                    mousePos - myCamera.transform.position,
                    out var hit,
                    100f,
                    layer
                )
            )
            {
                mousePos = hit.point;
            }
            transform.position = mousePos;
            Debug.DrawRay(mousePos, (mousePos - myCamera.transform.position) * 100f, Color.red);
        }
    }

    public void DiscColor(List<Material> material)
    {
        if (material is null)
        {
            disc.gameObject.SetActive(false);
            return;
        }

        disc.GetComponent<MeshRenderer>().SetMaterials(material);
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

    public virtual void DestroyTrafficTool()
    {
        eventBus.Send(new BudgetCost() { trafficTool = this, intSign = -1 });
        Destroy(this.gameObject);
    }

    public void ShowUI(bool isShowed)
    {
        traficToolCanvas.enabled = isShowed;
        outline.enabled = isShowed;
    }

    private void OnShowUIGameStarted()
    {
        ShowUI(false);
    }

    virtual public void OnSnap() { }
}
