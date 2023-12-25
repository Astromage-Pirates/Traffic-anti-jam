using System.ComponentModel;
using AstroPirate.DesignPatterns;
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
    private PathType pathType;

    [SerializeField]
    private Color color;

    [field: SerializeField]
    public int MaxVehicleEfficiency { get; private set; }

    /// <summary>
    /// Whether <see cref="Path"/> is available for <see cref="Vehicle"/> to move.
    /// </summary>
    [field: SerializeField]
    public bool Available { get; private set; } = true;

    [Header("Path Render")]
    [SerializeField]
    private MeshRenderer meshRenderer;

    /// <summary>
    /// The <see cref="UnityEngine.MeshFilter"/> of this <see cref="GameObject"/>.
    /// </summary>
    [field: SerializeField]
    public MeshFilter MeshFilter { get; private set; }
    private IEventBus eventBus;
    private bool isLevelPlayed;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out eventBus);
        eventBus.Register<LevelStateChanged>(OnLevelStateChanged);
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<LevelStateChanged>(OnLevelStateChanged);
    }

    private void Start()
    {
        meshRenderer.material.color = color;
    }

    private void Update()
    {
        meshRenderer.enabled = !isLevelPlayed;
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

    private void OnLevelStateChanged(LevelStateChanged levelState)
    {
        isLevelPlayed = levelState.IsPlay;
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
    /// Evaluates the tangent vector of a point, t, on a spline in world space.
    /// </summary>
    /// <param name="t">A value between 0 and 1 representing a percentage of entire spline.</param>
    /// <returns>The computed tangent vector.</returns>
    public Vector3 EvaluateTangent(float t)
    {
        return Container.EvaluateTangent(t);
    }

    /// <summary>
    /// Evaluates the up vector of a point, t, on a spline in world space.
    /// </summary>
    /// <param name="t">A value between 0 and 1 representing a percentage of entire spline.</param>
    /// <returns>The computed up direction.</returns>
    public Vector3 EvaluateUpVector(float t)
    {
        return Container.EvaluateUpVector(t);
    }

    /// <summary>
    /// The length of this <see cref="Path"/>.
    /// </summary>
    public float Length => Container.Spline.GetLength();
}
