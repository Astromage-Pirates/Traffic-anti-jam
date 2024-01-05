using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Edge
{
	[field: SerializeField] public PathNode From { get; private set; }
	[field: SerializeField] public PathNode To { get; private set; }
	[field: SerializeField] public float Weight { get; set; }
}
