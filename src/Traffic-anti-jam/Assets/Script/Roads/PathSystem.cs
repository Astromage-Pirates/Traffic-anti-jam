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

    [Header("Warning Message")]
    [SerializeField]
    private Canvas cnv_Warning;

    /// <summary>
    /// Get list of available <see cref="Path"/>s.
    /// </summary>
    public Path[] AvailablePaths => Paths.Where(s => s.Available).ToArray();

    public Path AvailablePath => Paths.FirstOrDefault(s => s.Available);

    private Path initialAvailablePath;

    private void Start()
    {
        initialAvailablePath = AvailablePath;
    }

    private void Update()
    {
        if (AvailablePaths.Length > 1)
        {
            foreach (var path in AvailablePaths)
            {
                if (path.InitAvailable)
                {
                    continue;
                }
                path.Available = false;
            }
        }

        if (cnv_Warning)
        {
            cnv_Warning.enabled = AvailablePaths.IsEmpty();
            var pathPosition = initialAvailablePath.EvaluatePosition(1);
            cnv_Warning.transform.position = new Vector3(
                pathPosition.x,
                cnv_Warning.transform.position.y,
                pathPosition.z
            );
        }
    }

#if UNITY_EDITOR
    [NaughtyAttributes.Button]
    private void AddNewPath()
    {
        var path = Instantiate(pathPrefab, transform);
        path.MeshFilter.mesh = null;
    }
#endif
}
