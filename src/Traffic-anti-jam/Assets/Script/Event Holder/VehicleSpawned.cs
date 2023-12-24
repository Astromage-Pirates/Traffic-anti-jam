using AstroPirate.DesignPatterns;

/// <summary>
/// Event for when <see cref="Vehicle"/> spawned.
/// </summary>
public class VehicleSpawned : EventContext
{
    public int CurrentVehicleCount;
}
