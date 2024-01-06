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
    private const float GoodEfficiencyPercentage = 1f / 2f;

    [SerializeField]
    private LevelManager levelManager;

    [SerializeField]
    private Slider sld_EfficencyBar;

    [SerializeField]
    private Image img_Fill;

    [SerializeField]
    private float transitionDuration = 0.2f;

    [SerializeField]
    private EfficiencyColor efficiencyColor;

    private int efficiencyVehicleCount;
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

    private IEventBus eventBus;

    private bool isPlayed = false;

    private void Awake()
    {
        GlobalServiceContainer.Resolve<IEventBus>(out eventBus);
        eventBus.Register<LevelStateChanged>(OnStateChanged);
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<LevelStateChanged>(OnStateChanged);
    }

    private void Start()
    {
        if (levelManager.paths != null)
        {
            efficiencyVehicleCount = levelManager.paths.Sum(x => x.MaxCar);
            img_Fill.color = efficiencyColor.Good;
        }
    }

    private void Update()
    {
        if (isPlayed)
        {
            var worstEfficiency = (efficiencyVehicleCount + 1) * 3f / 2f;
            efficiencyPercentage =
                1f
                - levelManager.paths.Sum(x => x.CarCount)
                    / worstEfficiency;

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
    }

    private void OnStateChanged(LevelStateChanged levelStateChanged)
    {
        isPlayed = levelStateChanged.IsPlay;
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
