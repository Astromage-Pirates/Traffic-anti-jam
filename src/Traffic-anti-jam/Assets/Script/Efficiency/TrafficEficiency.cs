using System.Linq;
using AstroPirate.DesignPatterns;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TrafficEficiency : MonoBehaviour
{
    private const float BadEfficiencyPercentage = 1 / 3;

    [SerializeField]
    private PathSystem pathSystem;

    [SerializeField]
    private Slider sld_EfficencyBar;

    [SerializeField]
    private Image img_Fill;

    [SerializeField]
    private float transitionDuration = 0.2f;

    private int efficiencyVehicleCount;
    private IEventBus eventBus;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out eventBus);
        eventBus.Register<VehicleSpawned>(OnVehicleSpawned);
    }

    private void Start()
    {
        efficiencyVehicleCount = pathSystem.AvailablePaths.Sum(x => x.MaxVehicleEfficiency);
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
            img_Fill.color = Color.red;
        }
    }
}
