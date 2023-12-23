using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Enumeration type of <see cref="Path"/>.
/// </summary>
public enum PathType
{
    Straight,
    TurnRight,
    TurnLeft
}

/// <summary>
/// The <see cref="Path"/> for <see cref="Vehicle"/> to move on.
/// </summary>
[RequireComponent(typeof(SplineContainer), typeof(SplineExtrude))]
public class Path : MonoBehaviour
{
    /// <summary>
    /// The <see cref="SplineContainer"/> that contains defined <see cref="Path"/>.
    /// </summary>
    [field: SerializeField]
    public SplineContainer Container { get; private set; }

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private PathType pathType;

    [SerializeField]
    private Color color;

    /// <summary>
    /// Whether <see cref="Path"/> is available for <see cref="Vehicle"/> to move.
    /// </summary>
    [field: SerializeField]
    public bool Available { get; private set; } = true;

    private void Start()
    {
        meshRenderer.material.color = color;
    }

    private void Update()
    {
        // TODO: [VD] check if game is stop
        meshRenderer.enabled = Available;
    }

    private void OnTriggerEnter(Collider other)
    {
        Available = pathType switch
        {
            // TODO: [VD] check for collision with no Turn Left sign.
            PathType.TurnLeft
                => false,
            // TODO: [VD] check for collision with no Turn Right sign.
            PathType.TurnRight
                => false,
            // TODO: [VD] check for collision with no Straight sign.
            PathType.Straight
                => false,
            _ => throw new InvalidEnumArgumentException()
        };
    }

    private void OnTriggerExit(Collider other)
    {
        Available = pathType switch
        {
            // TODO: [VD] check for collision with no Turn Left sign.
            PathType.TurnLeft
                => true,
            // TODO: [VD] check for collision with no Turn Right sign.
            PathType.TurnRight
                => true,
            // TODO: [VD] check for collision with no Straight sign.
            PathType.Straight
                => true,
            _ => throw new InvalidEnumArgumentException()
        };
    }

    /// <summary>
    /// Get the number of <see cref="Vehicle"/> on the current <see cref="Path"/>.
    /// </summary>
    public Vehicle[] VehiclesOnPath => GetComponentsInChildren<Vehicle>();

    /// <summary>
    /// Return an interpolated position at ratio t.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public Vector3 EvaluatePosition(float t)
    {
        return Container.EvaluatePosition(t);
    }

    /// <summary>
    /// The length of this <see cref="Path"/>.
    /// </summary>
    public float Length => Container.Spline.GetLength();
}
