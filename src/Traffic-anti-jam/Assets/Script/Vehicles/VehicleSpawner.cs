using AstroPirate.DesignPatterns;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// A helper component to spawn <see cref="Vehicle"/>.
/// </summary>
public class VehicleSpawner : MonoBehaviour
{
    [SerializeField]
    private PathSystem pathSystem;

    [SerializeField]
    private float spawningSeconds = 2f;

    [Tooltip("The distance to check for spawning to prevent from it's on top of other object.")]
    [SerializeField]
    private float spawningDistance = 1f;

    [SerializeField]
    private Vehicle[] vehiclePrefabs;

    [SerializeField]
    private bool isLevelPlayed;

    private IEventBus eventBus;
    private int currentVehicleCount;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out eventBus);
        eventBus.Register<PlayStageEnded>(OnPlayStageEnded);
        eventBus.Register<LevelStateChanged>(OnLevelStateChanged);
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<PlayStageEnded>(OnPlayStageEnded);
        eventBus.UnRegister<LevelStateChanged>(OnLevelStateChanged);
    }

    private void OnLevelStateChanged(LevelStateChanged levelState)
    {
        isLevelPlayed = levelState.IsPlay;
    }

    private void OnPlayStageEnded(PlayStageEnded playState)
    {
        isLevelPlayed = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnVehicle), 0f, spawningSeconds);
    }

    private void SpawnVehicle()
    {
        if (isLevelPlayed)
        {
            var availablePaths = pathSystem.AvailablePaths;
            var pathIndex = Random.Range(0, availablePaths.Length);

            if (ShouldSpawnVehicle(availablePaths[pathIndex]))
            {
                var vehicleIndex = Random.Range(0, vehiclePrefabs.Length);

                var vehicle = Instantiate(
                    vehiclePrefabs[vehicleIndex],
                    availablePaths[pathIndex].EvaluatePosition(0),
                    Quaternion.identity,
                    availablePaths[pathIndex].transform
                );

                vehicle.Path = availablePaths[pathIndex];

                currentVehicleCount += 1;

                eventBus.Send(new VehicleSpawned { CurrentVehicleCount = currentVehicleCount });
            }
        }
    }

    private bool ShouldSpawnVehicle(Path path)
    {
        if (path.VehiclesOnPath.IsEmpty())
        {
            return true;
        }

        var pathStartPosition = path.EvaluatePosition(0);

        foreach (var vehicle in path.VehiclesOnPath)
        {
            var distance = Vector3.Distance(vehicle.transform.position, pathStartPosition);

            if (distance < spawningDistance)
            {
                return false;
            }
        }

        return true;
    }
}
