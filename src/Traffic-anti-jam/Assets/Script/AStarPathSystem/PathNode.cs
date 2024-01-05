using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    [SerializeField]
    private List<Edge> edges;

	[SerializeField]
	private float radius;
	public List<Edge> Edges => edges;
	public float Radius => radius;
	
	public bool ReachedThisNode(Vector3 position)
	{
		return Vector3.Distance(transform.position, position) < radius;
	}	
}
