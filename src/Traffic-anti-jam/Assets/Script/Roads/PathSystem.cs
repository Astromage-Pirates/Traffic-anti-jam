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

    /// <summary>
    /// Get list of available <see cref="Path"/>s.
    /// </summary>
    public Path[] AvailablePaths => Paths.Where(s => s.Available).ToArray();

#if UNITY_EDITOR
    [NaughtyAttributes.Button]
    private void AddNewPath()
    {
        Instantiate(pathPrefab, transform);
    }
#endif
}
