using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathSystem : MonoBehaviour
{
    /// <summary>
    /// List of <see cref="Path"/>s available.
    /// </summary>
    public List<Path> Paths { get; private set; }

    [SerializeField]
    private Path pathPrefab;

    /// <summary>
    /// Get list of available <see cref="Path"/>s.
    /// </summary>
    public Path[] AvailablePaths => Paths.Where(s => s.Available).ToArray();

#if UNITY_EDITOR
    [NaughtyAttributes.Button]
    private void AddNewPath()
    {
        var path = Instantiate(pathPrefab, transform);

        Paths.Add(path);
    }
#endif
}
