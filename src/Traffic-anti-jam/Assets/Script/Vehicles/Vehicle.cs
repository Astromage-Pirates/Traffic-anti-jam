using AstroPirate.DesignPatterns;
using UnityEngine;

/// <summary>
/// Represents transporting means.
/// </summary>
public class Vehicle : MonoBehaviour
{
    private const float MinVelocity = 15f;
    private const float MaxPercentage = 1f;
    private const float NextPositionOffset = 0.05f;

    [Tooltip("Distance to check for collision with other object.")]
    [SerializeField]
    private float collisionCheckDistance = 0.2f;

    [Tooltip("Distance to check for collision with other object.")]
    [SerializeField]
    private float minDistance = 0.2f;

    [Tooltip("Distance to check for collision with other object.")]
    [SerializeField]
    private Vector3 boxHalfExtent;

    [SerializeField]
    private float velocity = 0.6f;

    [SerializeField]
    private Transform raycastAnchor;

    /// <summary>
    /// The path for <see cref="Vehicle"/> to move.
    /// </summary>
    public Path Path { get; set; }

    /// <summary>
    /// The <see cref="Pool{T}"/> that contains this <see cref="Vehicle"/>.
    /// </summary>
    public Pool<Vehicle> Pool { get; set; }

    private float distancePercentage;
    private bool stopBySign;
    private int currentVehicleCount;
    private IEventBus eventBus;
    private float velocityModifier;
    private bool isLevelPlayed;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out eventBus);
        eventBus.Register<VehicleSpawned>(OnVehicleSpawned);
        eventBus.Register<LevelStateChanged>(OnLevelStageChanged);
        eventBus.Register<PlayStageEnded>(OnPlayStageEnded);
    }

    private void OnLevelStageChanged(LevelStateChanged levelState)
    {
        isLevelPlayed = levelState.IsPlay;
    }

    private void OnPlayStageEnded(PlayStageEnded ended)
    {
        isLevelPlayed = false;
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<VehicleSpawned>(OnVehicleSpawned);
    }

    private void Start()
    {
        distancePercentage = 0f;
    }

    private void Update()
    {
        Move();
        CheckIsAtDestination();
    }

    private void FixedUpdate()
    {
        CheckForCollision();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MinSpeedSign>())
        {
            velocity = MinVelocity;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<TrafficLight>(out var trafficLight))
        {
            if (trafficLight.LightStage == TrafficLight.LightMode.red)
            {
                stopBySign = true;
            }
            else
            {
                stopBySign = false;
            }
        }
    }

    private void CheckForCollision()
    {
        var collisionResult = Physics.BoxCast(
            raycastAnchor.position - raycastAnchor.forward * raycastAnchor.localPosition.y,
            boxHalfExtent,
            raycastAnchor.forward,
            out var hitInfo,
            raycastAnchor.rotation,
            collisionCheckDistance,
            LayerMask.GetMask("Vehicle")
        );

        if (collisionResult)
        {
            if (hitInfo.distance < minDistance)
            {
                velocityModifier = 0.0f;
            }
            else
            {
                velocityModifier = hitInfo.distance / collisionCheckDistance;
            }
        }
        else
        {
            velocityModifier = 1.0f;
        }
    }

    private void OnDrawGizmos()
    {
        var collisionResult = Physics.BoxCast(
            raycastAnchor.position,
            boxHalfExtent,
            raycastAnchor.forward,
            out var hitInfo,
            raycastAnchor.rotation,
            collisionCheckDistance,
            LayerMask.GetMask("Vehicle")
        );

        Gizmos.DrawWireSphere(raycastAnchor.position + raycastAnchor.forward * minDistance, 0.1f);

        if (collisionResult)
        {
            Gizmos.color = Color.green;
            var old = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(
                raycastAnchor.position,
                raycastAnchor.rotation,
                Vector3.one
            );
            Gizmos.DrawWireCube(Vector3.forward * hitInfo.distance, boxHalfExtent * 2.0f);
            Gizmos.matrix = old;
            Gizmos.DrawRay(raycastAnchor.position, raycastAnchor.forward * hitInfo.distance);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(raycastAnchor.position, raycastAnchor.forward * collisionCheckDistance);
        }
    }

    private void Move()
    {
        if (stopBySign)
        {
            return;
        }

        distancePercentage += velocityModifier * velocity * Time.deltaTime / Path.Length;

        var currentPosition = Path.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        var nextPosition = Path.EvaluatePosition(distancePercentage + NextPositionOffset);

        Vector3 direction = nextPosition - currentPosition;

        var forward = Vector3.Normalize(Path.EvaluateTangent(distancePercentage));
        var up = Path.EvaluateUpVector(distancePercentage);

        transform.rotation = Quaternion.LookRotation(forward, up);
    }

    private void CheckIsAtDestination()
    {
        if (IsAtDestination)
        {
            distancePercentage = 0f;
            Pool.Release(this);

            if (isLevelPlayed)
            {
                eventBus.Send(new VehicleSpawned { CurrentVehicleCount = currentVehicleCount - 1 });
            }
        }
    }

    private void OnVehicleSpawned(VehicleSpawned vehicleSpawned)
    {
        currentVehicleCount = vehicleSpawned.CurrentVehicleCount;
    }

    private bool IsAtDestination => distancePercentage >= MaxPercentage;
}
