using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour
{
    [SerializeField]
    private SplineContainer path;

    [SerializeField]
    private Rigidbody rgbody;

    [SerializeField]
    private float speed;

    private bool hasCrashed = false;

    private void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Vehicle>())
        {
            hasCrashed = true;

            // TODO: [VD] set current game state to game over
        }
    }

    private void Move()
    {
        if (hasCrashed)
        {
            return;
        }

        var nativePath = new NativeSpline(path.Spline);
        var localToWorldMatrix = path.transform.localToWorldMatrix;
        var worldToLocalMatrix = path.transform.worldToLocalMatrix;
        float _ = SplineUtility.GetNearestPoint(
            nativePath,
            worldToLocalMatrix.MultiplyPoint(transform.position),
            out var nearest,
            out var t
        );
        var forward = Vector3.Normalize(path.EvaluateTangent(t));
        var up = path.EvaluateUpVector(t);

        rgbody.position = localToWorldMatrix.MultiplyPoint(nearest);
        rgbody.rotation = Quaternion.LookRotation(forward, up);
        rgbody.velocity = rgbody.velocity.magnitude * transform.forward;
        rgbody.AddForce(transform.forward * speed);
    }
}
