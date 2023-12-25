using System.Linq;
using AstroPirate.DesignPatterns;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The status of traffic efficiency.
/// </summary>
public enum EfficiencyStatus
{
    Bad,
    Good,
}

public class TrafficEfficiency : MonoBehaviour
{
    private const float BadEfficiencyPercentage = 1f / 3f;

    [SerializeField]
    private PathSystem pathSystem;

    [SerializeField]
    private Slider sld_EfficencyBar;

    [SerializeField]
    private Image img_Fill;

    [SerializeField]
    private Material badEfficiencyMaterial;

    [SerializeField]
    private Material goodEfficiencyMaterial;

    [SerializeField]
    private float transitionDuration = 0.2f;

    private int efficiencyVehicleCount;
    private IEventBus eventBus;

    /// <summary>
    /// The status of traffic efficiency.
    /// </summary>
    public EfficiencyStatus EfficiencyStatus;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out eventBus);
        eventBus.Register<VehicleSpawned>(OnVehicleSpawned);
    }

    private void Start()
    {
        efficiencyVehicleCount = pathSystem.AvailablePaths.Sum(x => x.MaxVehicleEfficiency);
    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<VehicleSpawned>(OnVehicleSpawned);
    }

    private void OnVehicleSpawned(VehicleSpawned vehicleSpawned)
    {
        var leastEfficiency = (efficiencyVehicleCount + 1) * 3f / 2f;
        var efficiencyPercentage = 1f - vehicleSpawned.CurrentVehicleCount / leastEfficiency;

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
            img_Fill.material = badEfficiencyMaterial;
        }
        else
        {
            img_Fill.material = goodEfficiencyMaterial;
        }
    }
}
