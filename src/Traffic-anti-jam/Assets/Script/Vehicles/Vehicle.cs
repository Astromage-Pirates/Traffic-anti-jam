using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour
{
    private const float MaxPercentage = 1f;
    private const float NextPositionOffset = 0.05f;

    [SerializeField]
    private SplineContainer path;

    [SerializeField]
    private Rigidbody rgbody;

    [SerializeField]
    private float velocity = 0.6f;

    private bool hasCrashed = false;

    private float distancePercentage;

    private void Update()
    {
        // TODO: [VD] check if game state is not game over.
        Move();
        CheckIsAtDestination();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Vehicle>())
        {
            hasCrashed = true;

            // TODO: [VD] set current game state to game over.
        }
    }

    private void Move()
    {
        if (hasCrashed)
        {
            return;
        }

        distancePercentage += velocity * Time.deltaTime / path.CalculateLength();

        var currentPosition = path.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        var nextPosition = path.EvaluatePosition(distancePercentage + NextPositionOffset);
        var direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction);
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
