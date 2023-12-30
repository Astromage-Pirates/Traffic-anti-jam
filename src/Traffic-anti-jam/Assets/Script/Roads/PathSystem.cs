using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathSystem : MonoBehaviour
{
    [SerializeField]
    private Path pathPrefab;

    /// <summary>
    /// List of available <see cref="Path"/>s.
    /// </summary>
    [field: SerializeField]
    public List<Path> Paths { get; private set; }

    [field: SerializeField]
    public int MaxVehicleEfficiency { get; private set; }

    /// <summary>
    /// Get list of available <see cref="Path"/>s.
    /// </summary>
    public Path[] AvailablePaths => Paths.Where(s => s.Available).ToArray();

    public Path AvailablePath => Paths.FirstOrDefault(s => s.Available);

#if UNITY_EDITOR
    [NaughtyAttributes.Button]
    private void AddNewPath()
    {
        var path = Instantiate(pathPrefab, transform);
        path.MeshFilter.mesh = null;
    }
#endif
}
