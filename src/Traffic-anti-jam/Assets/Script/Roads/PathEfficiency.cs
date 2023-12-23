using UnityEngine;

/// <summary>
/// Represents <see cref="global::Path"/> efficiency limit.
/// </summary>
public class PathEfficiency : MonoBehaviour
{
    [field: SerializeField]
    public Path Path { get; private set; }

    /// <summary>
    /// The number of vehicles that satisfy good traffic efficiency.
    /// </summary>
    [Tooltip("The number of vehicles that satisfy good traffic efficiency.")]
    [field: SerializeField]
    public int MaxVehicleCount { get; private set; }
}
