using AstroPirate.DesignPatterns;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Represents transporting means.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour
{
    private const float MinVelocity = 15f;
    private const float MaxPercentage = 1f;
    private const float NextPositionOffset = 0.05f;

    [SerializeField]
    private Rigidbody rgbody;

    [Tooltip("Distance to check for collision with other object.")]
    [SerializeField]
    private float collisionCheckDistance = 0.2f;

    [SerializeField]
    private float velocity = 0.6f;

    [SerializeField]
    private Transform raycastAnchor;

    /// <summary>
    /// The path for <see cref="Vehicle"/> to move.
    /// </summary>
    public Path Path { get; set; }

    private float distancePercentage;
    private bool stopByCollision;
    private bool stopBySign;
    private int currentVehicleCount;
    private IEventBus eventBus;
    private float oldVelocity;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out eventBus);
        eventBus.Register<VehicleSpawned>(OnVehicleSpawned);
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
        // TODO: [VD] check if game state is not game over.
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

    // private void OnTriggerExit(Collider other)
    // {
    //     velocity = oldVelocity;
    // }

    private void CheckForCollision()
    {
        if (
            Physics.Raycast(
                raycastAnchor.position,
                raycastAnchor.forward,
                out var hitInfo,
                collisionCheckDistance,
                LayerMask.GetMask("Vehicle")
            )
        )
        {
            stopByCollision = true;
        }
        else
        {
            stopByCollision = false;
        }
    }

    private void Move()
    {
        if (stopByCollision || stopBySign)
        {
            return;
        }

        distancePercentage += velocity * Time.deltaTime / Path.Length;

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
            Destroy(gameObject);
            eventBus.Send(new VehicleSpawned { CurrentVehicleCount = currentVehicleCount - 1 });
        }
    }

    private void OnVehicleSpawned(VehicleSpawned vehicleSpawned)
    {
        currentVehicleCount = vehicleSpawned.CurrentVehicleCount;
    }

    private bool IsAtDestination => distancePercentage >= MaxPercentage;
}
