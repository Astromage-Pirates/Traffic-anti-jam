using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// A helper component to spawn <see cref="Vehicle"/>.
/// </summary>
public class VehicleSpawner : MonoBehaviour
{
    [SerializeField]
    private float spawningSeconds = 3f;

    [Tooltip("The distance to check for spawning to prevent from it's on top of other object.")]
    [SerializeField]
    private float spawningDistance = 1f;

    [SerializeField]
    private PathSystem pathSystem;

    [SerializeField]
    private Vehicle[] vehiclePrefabs;

    private Vehicle lastSpawnedVehicle;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnVehicle), 0f, spawningSeconds);
    }

    private void SpawnVehicle()
    {
        var availablePaths = pathSystem.AvailablePaths;
        var pathIndex = Random.Range(0, availablePaths.Length);

        if (ShouldSpawnVehicle(availablePaths[pathIndex]))
        {
            var vehicleIndex = Random.Range(0, vehiclePrefabs.Length);

            lastSpawnedVehicle = Instantiate(
                vehiclePrefabs[vehicleIndex],
                availablePaths[pathIndex].transform,
                true
            );

            lastSpawnedVehicle.Path = availablePaths[pathIndex];
        }
    }

    private bool ShouldSpawnVehicle(Path path)
    {
        if (path.VehiclesOnPath.IsEmpty())
        {
            return true;
        }

        var pathStartPosition = path.EvaluatePosition(0);
        var distance = Vector3.Distance(lastSpawnedVehicle.transform.position, pathStartPosition);

        return distance >= spawningDistance;
    }
}
