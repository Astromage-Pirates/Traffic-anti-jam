using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// A helper component to spawn <see cref="Vehicle"/>.
/// </summary>
public class VehicleSpawner : MonoBehaviour
{
    [SerializeField]
    private float spawningSeconds = 2f;

    [Tooltip("The distance to check for spawning to prevent from it's on top of other object.")]
    [SerializeField]
    private float spawningDistance = 0.5f;

    [SerializeField]
    private PathSystem pathSystem;

    [SerializeField]
    private Vehicle[] vehiclePrefabs;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnVehicle), 0f, spawningSeconds);
    }

    private void SpawnVehicle()
    {
        var availablePaths = pathSystem.AvailablePaths;
        var vehicleIndex = Random.Range(0, vehiclePrefabs.Length);
        var pathIndex = Random.Range(0, availablePaths.Length);

        if (!ShouldSpawnVehicle(availablePaths[pathIndex]))
        {
            return;
        }

        var vehicle = Instantiate(
            vehiclePrefabs[vehicleIndex],
            availablePaths[pathIndex].transform,
            true
        );

        vehicle.Path = availablePaths[pathIndex];
    }

    private bool ShouldSpawnVehicle(Path path)
    {
        if (path.VehiclesOnPath.IsEmpty())
        {
            return true;
        }

        var lastSpawnVehicle = path.VehiclesOnPath.Last();
        var pathStartPosition = path.EvaluatePosition(0);

        return Vector3.Distance(lastSpawnVehicle.transform.position, pathStartPosition)
            >= spawningDistance;
    }
}
