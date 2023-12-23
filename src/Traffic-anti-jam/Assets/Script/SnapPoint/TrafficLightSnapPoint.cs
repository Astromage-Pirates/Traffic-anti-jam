using System;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using UnityEditor;
using UnityEngine;

public class TrafficLightSnapPoint : SnapPoint<TrafficLight>
{
    [SerializeField]
    private GameObject tlLocator_Prefab;

    [SerializeField]
    private int ammountOfSnapPoints;

    [SerializeField]
    private List<GameObject> TLLocators = new();

    private int reviewChecked = -1;

    private IEventBus eventBus;

    #region Editor
#if UNITY_EDITOR
    [NaughtyAttributes.Button]
    private void CreateSnapPoints()
    {
        if (TLLocators is not null || TLLocators.Count < 0)
        {
            foreach (GameObject tlLocator in TLLocators)
            {
                DestroyImmediate(tlLocator);
            }

            TLLocators.Clear();
        }

        for (int count = 0; count < ammountOfSnapPoints; count++)
        {
            var tlLocator = Instantiate(tlLocator_Prefab, gameObject.transform);
            TLLocators.Add(tlLocator);
        }

        reviewChecked *= -1;
    }

    [NaughtyAttributes.Button]
    private void ReviewSnapPoints()
    {
        bool isReviwed = (reviewChecked > 0);
        reviewChecked *= -1;
        foreach (GameObject tlLocator in TLLocators)
        {
            tlLocator.GetComponent<MeshRenderer>().enabled = isReviwed;
        }
    }
#endif
    #endregion

    private void Awake()
    {
        GlobalServiceContainer.Resolve<IEventBus>(out eventBus);
        eventBus.Register<TrafficLightUIInteracted>(OnTrafficLightSnapPointActive);
        OnTrafficLightSnapPointActive(
            new TrafficLightUIInteracted() { isSnapPointActive = false, isToolBarBtnActive = true }
        );
    }

    private void OnTrafficLightSnapPointActive(TrafficLightUIInteracted trafficLightUIInteracted)
    {
        myCollider.enabled = trafficLightUIInteracted.isSnapPointActive;
        myMeshRenderer.enabled = trafficLightUIInteracted.isSnapPointActive;
    }

    protected override void OnSnap(TrafficLight trafficLight)
    {
        trafficLight.isSnaped = true;
        eventBus.Send(new TrafficToolUIInteracted());
        eventBus.Send(new BudgetCost() { trafficTool = trafficLight, intSign = 1 });
        trafficLight.transform.position = transform.position;

        List<TrafficLight> tempList = new();

        foreach (GameObject tlLocator in TLLocators)
        {
            Vector3 tlPos = tlLocator.transform.position;
            int rotateY = (int)tlLocator.GetComponent<DirectionPoint>().direction;
            var newTrafficLight = Instantiate(trafficLight, tlPos, Quaternion.Euler(0, rotateY, 0));
            newTrafficLight.trafficLightHolder = trafficLight;
            newTrafficLight.disc.SetActive(false);
            tempList.Add(newTrafficLight);
        }

        trafficLight.trafficLightChildren = tempList;

        eventBus.Send(
            new TrafficLightUIInteracted() { isSnapPointActive = false, isToolBarBtnActive = true }
        );

        trafficLight.TrafficLightHolderOff();
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<TrafficLightUIInteracted>(OnTrafficLightSnapPointActive);
    }
}
