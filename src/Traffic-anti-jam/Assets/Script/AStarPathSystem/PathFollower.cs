using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
	[SerializeField]
	private WheelBaseVehicle vehicle;
	
	[SerializeField]
	private BetterPath route;
	
	[SerializeField]
	private float steerSpeed;
	
	[SerializeField]
	private float velocity;

	[SerializeField,Min(2)]
	private int RayCount;
	
	[SerializeField]
	private int RaySpacing;

	[SerializeField]
	private float DistanceThreshHold;
	
	[SerializeField]
	private float DistanceToOtherCar;

	[SerializeField]
	private Transform raycastAnchor;


	private PathNode currentNode;
	private PathNode lastNode;

	Vector3 CurrentDirection = Vector3.zero;

	private float input;

	private bool isStarted = false;

	private void Start()
	{
	}

	private void Update()
	{
		if(currentNode == null)
		{
			if(isStarted)
			{
				Destroy(gameObject);
			}
			else
			{
				currentNode = route.GetStartNode();
				isStarted = true;
			}
		}
		else if(currentNode.ReachedThisNode(transform.position))
		{
			lastNode = currentNode;
			currentNode = route.GetNextNode(currentNode);
			accumulatedError = 0.0f;
		}
	}

	float accumulatedError = 0.0f;
	private void FixedUpdate()
	{
		if (currentNode == null)
		{
			return;
		}

		if (lastNode == null) { return; }
		if(stopBySign) { return; }

		Move();

	}

	void Move()
	{
		Vector3 dir = GetCurrentSegmentDirection();
		Vector3 dirToTarget = currentNode.transform.position - transform.position;
		Vector3 right = Vector3.Cross(dir, Vector3.up).normalized;
		float dot = Vector3.Dot(right, transform.position - lastNode.transform.position);
		accumulatedError += dot;

		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToTarget.normalized - right * dot),
			Time.fixedDeltaTime * steerSpeed);


		float minAngle = RaySpacing / (RayCount -1);
		float startAngle = RaySpacing * 0.5f;
		float speedModifier = 1.0f;
		for (int i = 0; i < RayCount; i++)
		{
			var rotation = raycastAnchor.rotation * Quaternion.Euler(0.0f, minAngle * i - startAngle, 0.0f);
			if(Physics.Raycast(raycastAnchor.position,rotation*Vector3.forward,out var hit,DistanceThreshHold,1 << LayerMask.NameToLayer("Vehicle")))
			{
				Debug.DrawRay(raycastAnchor.position, rotation * Vector3.forward * hit.distance, Color.red);
				if(hit.distance < DistanceToOtherCar)
				{
					speedModifier = 0.0f;
				}
				else
				{
					speedModifier = hit.distance / DistanceThreshHold;
				}
				break;
			}
		}

		transform.Translate(Vector3.forward * speedModifier * velocity * Time.fixedDeltaTime);
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
	public float angle;
	private bool stopBySign = false;

	private void OnDrawGizmos()
	{
		
		if (currentNode == null) return;
		Handles.color = Color.blue;
		Vector3 targetPos = currentNode.transform.position;
		Vector3 pos = transform.position;
		Handles.DrawLine(pos, targetPos);
		var labelStyle = new GUIStyle();
		labelStyle.normal.textColor = Color.red;
		labelStyle.alignment = TextAnchor.MiddleCenter;
		Handles.Label(0.5f * (targetPos + pos), Vector3.Distance(targetPos, pos).ToString(), labelStyle);

		if(lastNode == null) { return; }
		Vector3 dir = GetCurrentSegmentDirection();
		Vector3 right = Vector3.Cross(dir, Vector3.up).normalized;
		float dot = Vector3.Dot(dir.normalized, transform.position - lastNode.transform.position);

		
	}

	Vector3 GetCurrentSegmentDirection()
	{
		if(lastNode == null || currentNode == null) return Vector3.zero;
		return lastNode.transform.position - currentNode.transform.position;
	}

	public void SetRoute(BetterPath route)
	{
		this.route = route;
	}
}
