using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using AstroPirate.DesignPatterns;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TrafficLight : TrafficTool
{
    #region TrafficLightHolder

    [SerializeField]
    private GameObject TL_Model;

    [SerializeField]
    private GameObject grp_Light;

    [SerializeField]
    private BoxCollider areaZone;

    [SerializeField]
    private GameObject trafficLightCanvas;
    public List<TrafficLight> trafficLightChildren = new();

    public TrafficLight trafficLightHolder;

    public void TrafficLightHolderOff()
    {
        TL_Model.SetActive(false);
        grp_Light.SetActive(false);
        trafficLightCanvas.SetActive(false);
        areaZone.enabled = false;
        disc.SetActive(false);
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public override void DestroyTrafficTool()
    {
        if (trafficLightHolder)
        {
            eventBus.Send(new BudgetCost() { trafficTool = trafficLightHolder, intSign = -1 });
            Destroy(trafficLightHolder.gameObject);
        }
    }

    #endregion

    public enum LightMode
    {
        green,
        red
    }

    [SerializeField]
    private GameObject redLight;

    [SerializeField]
    private GameObject greenLight;

    [SerializeField]
    private GameObject redLightUI;

    [SerializeField]
    private GameObject greenLightUI;

    [SerializeField]
    private LightMode lightStage;

    [SerializeField]
    private int lightStageDuration;

    private CancellationToken cts;

    public LightMode LightStage
    {
        get => lightStage;
    }

    protected override void Awake()
    {
        GreenLigthStage();
        base.Awake();
        eventBus.Register<LevelStateChanged>(OnLevelStateChanged);
    }

    private LightMode GreenLigthStage()
    {
        greenLight.SetActive(true);
        redLight.SetActive(false);
        redLightUI.SetActive(true);
        greenLightUI.SetActive(false);
        return lightStage = LightMode.green;
    }

    public void OnBtnClickGreenLightStage()
    {
        GreenLigthStage();
    }

    private LightMode RedLightStage()
    {
        redLight.SetActive(true);
        greenLight.SetActive(false);
        greenLightUI.SetActive(true);
        redLightUI.SetActive(false);
        return lightStage = LightMode.red;
    }

    public void OnBtnClickRedlightStage()
    {
        RedLightStage();
    }

    private void OnLevelStateChanged(LevelStateChanged levelStateChanged)
    {
        ChangeLightStage(levelStateChanged).Forget();
    }

    private async UniTaskVoid ChangeLightStage(LevelStateChanged levelStateChanged)
    {
        cts = new();

        while (levelStateChanged.IsPlay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(lightStageDuration), cancellationToken: cts);

            if (lightStage == LightMode.green)
            {
                RedLightStage();
            }
            else if (lightStage == LightMode.red)
            {
                GreenLigthStage();
            }
        }
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<LevelStateChanged>(OnLevelStateChanged);
        foreach (TrafficLight trafficLight in trafficLightChildren)
        {
            if(trafficLight.gameObject != null)
            {
                Destroy(trafficLight.gameObject);
            }
        }
    }
}
