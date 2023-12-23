using System.Linq;
using UnityEngine;

public class PathSystem : MonoBehaviour
{
    [field: SerializeField]
    public float PathDectectThreshold { get; set; } = 0.01f;

    [field: SerializeField]
    public Path[] Paths { get; private set; }

    /// <summary>
    /// Get list of available <see cref="Path"/>s.
    /// </summary>
    public Path[] AvailablePaths => Paths.Where(s => s.Available).ToArray();
}
