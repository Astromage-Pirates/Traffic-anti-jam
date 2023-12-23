using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour
{
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

    [SerializeField]
    private AudioSource audioSource;

    /// <summary>
    /// The path for <see cref="Vehicle"/> to move.
    /// </summary>
    public Path Path { get; set; }

    private float distancePercentage;
    private bool stopByCollision;
    private bool stopBySign;

    private bool isAccidentCalled;

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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Vehicle>())
        {
            isAccidentCalled = true;
            audioSource.Play();

            // TODO: [VD] set current game state to game over.
        }
    }

    private async void OnTriggerEnter(Collider other)
    {
        stopBySign = true;

        await UniTask.Delay(10000);

        stopBySign = false;
    }

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
        if (stopByCollision || stopBySign || isAccidentCalled)
        {
            return;
        }

        distancePercentage += velocity * Time.deltaTime / Path.Length;

        var currentPosition = Path.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        var nextPosition = Path.EvaluatePosition(distancePercentage + NextPositionOffset);

        Vector3 direction = nextPosition - currentPosition;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void CheckIsAtDestination()
    {
        if (IsAtDestination)
        {
            Destroy(gameObject);
        }
    }

    private bool IsAtDestination => distancePercentage >= MaxPercentage;
}
