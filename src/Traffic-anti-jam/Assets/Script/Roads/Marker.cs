using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField]
    private List<Marker> adjacentMarkers;

    /// <summary>
    /// Whether this <see cref="Marker"/> is open for connection.
    /// </summary>
    [field: SerializeField]
    public bool OpenForConnections { get; private set; }

    /// <summary>
    /// Get all of other adjacent <see cref="Marker"/>s positions.
    /// </summary>
    public List<Vector3> GetAdjacentPositions =>
        new List<Vector3>(adjacentMarkers.Select(x => x.transform.position).ToList());

    private void OnDrawGizmos()
    {
        if (Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.red;

            if (adjacentMarkers != null && !adjacentMarkers.IsEmpty())
            {
                foreach (var item in adjacentMarkers)
                {
                    Gizmos.DrawLine(transform.position, item.transform.position);
                }
            }

            Gizmos.color = Color.white;
        }
    }
}
