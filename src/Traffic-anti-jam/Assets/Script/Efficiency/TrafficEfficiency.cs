using System;
using System.Collections.Generic;
using System.Linq;
using AstroPirate.DesignPatterns;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The status of traffic efficiency.
/// </summary>
public enum EfficiencyStatus
{
    Bad,
    Medium,
    Good,
}

/// <summary>
/// The traffic efficiency.
/// </summary>
public class TrafficEfficiency : MonoBehaviour
{
    [Serializable]
    private struct EfficiencyColor
    {
        public Color Bad;
        public Color Medium;
        public Color Good;
    }

    private const float BadEfficiencyPercentage = 1f / 3f;
    private const float GoodEfficiencyPercentage = 2f / 3f;

    [SerializeField]
    private PathSystem pathSystem;

    [SerializeField]
    private Slider sld_EfficencyBar;

    [SerializeField]
    private Image img_Fill;

    [SerializeField]
    private float transitionDuration = 0.2f;

    [SerializeField]
    private EfficiencyColor efficiencyColor;

    private int efficiencyVehicleCount;
    private IEventBus eventBus;
    private float efficiencyPercentage;

    /// <summary>
    /// The status of traffic efficiency.
    /// </summary>
    public EfficiencyStatus EfficiencyStatus
    {
        get
        {
            if (efficiencyPercentage <= BadEfficiencyPercentage)
            {
                return EfficiencyStatus.Bad;
            }
            else if (efficiencyPercentage <= GoodEfficiencyPercentage)
            {
                return EfficiencyStatus.Medium;
            }

            return EfficiencyStatus.Good;
        }
    }

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out eventBus);
        eventBus.Register<VehicleSpawned>(OnVehicleSpawned);
    }

    private void Start()
    {
        if (pathSystem)
        {
            efficiencyVehicleCount = pathSystem.AvailablePaths.Sum(x => x.MaxVehicleEfficiency);
            img_Fill.color = efficiencyColor.Good;
        }
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<VehicleSpawned>(OnVehicleSpawned);
    }

    private void OnVehicleSpawned(VehicleSpawned vehicleSpawned)
    {
        var leastEfficiency = (efficiencyVehicleCount + 1) * 3f / 2f;
        efficiencyPercentage = 1f - vehicleSpawned.CurrentVehicleCount / leastEfficiency;

        DOVirtual.Float(
            sld_EfficencyBar.value,
            efficiencyPercentage,
            transitionDuration,
            value =>
            {
                sld_EfficencyBar.value = value;
                ChangeSlideColor(value);
            }
        );
    }

    private void ChangeSlideColor(float value)
    {
        if (value <= BadEfficiencyPercentage)
        {
            img_Fill.color = efficiencyColor.Bad;
        }
        else if (value <= GoodEfficiencyPercentage)
        {
            img_Fill.color = efficiencyColor.Medium;
        }
        else
        {
            img_Fill.color = efficiencyColor.Good;
        }
    }
}
