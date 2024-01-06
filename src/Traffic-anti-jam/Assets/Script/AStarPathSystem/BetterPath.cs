using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BetterPath : MonoBehaviour
{

    [field: SerializeField]
    public int MaxCar { get; private set; }
    
    [field: SerializeField]
    public PathNode StartNode { get; private set; }

    [field: SerializeField]
    public PathNode EndNode { get; private set; }

    [SerializeField]
    private float dotSpacing;

    [SerializeField]
    private Vector3 LineOffset;

    [SerializeField]
    private MeshRenderer Sphere;

    [SerializeField]
    private Color pathColor;
    
    [SerializeField]
    private Material baseMat;

    private Material material;
    
    private List<PathNode> path;
    private List<MeshRenderer> Spheres = new();

    public List<PathNode> ShortestPath => path;

    public int CarCount;

	private void Start()
	{
        material = new Material(baseMat);
        material.color = pathColor;

        evaluateRoute();
	}
	[ContextMenu("Re-evaluate Path")]
	public void evaluateRoute()
    {
        HashSet<PathNode> evaluatedNode = new();
        Dictionary<PathNode,float> unevaluatedNode = new();
        Dictionary<PathNode, PathNode> path = new();
        bool found = false;
		unevaluatedNode.Add(StartNode,0);
        while(unevaluatedNode.Count > 0)
        {
            var currentNode = unevaluatedNode.OrderBy(node => node.Value).FirstOrDefault();
            unevaluatedNode.Remove(currentNode.Key);
            evaluatedNode.Add(currentNode.Key);
            if(currentNode.Key == EndNode)
            {
                found = true;
                break;
            }
            foreach(var child in currentNode.Key.Edges)
            {
                if (evaluatedNode.Contains(child.To)) continue;
                float distance = currentNode.Value + GetDistance(currentNode.Key,child.To) + child.Weight;
                if(unevaluatedNode.TryGetValue(child.To,out var f) )
                {
                    if(f > distance)
                    {
						unevaluatedNode[child.To] = distance;
						if (path.ContainsKey(child.To))
						{
							path[child.To] = currentNode.Key;
						}
					}
                }
                else
                {
                    unevaluatedNode.Add(child.To, distance);
                    path.Add(child.To, currentNode.Key);
                }
            }
        }
        this.path = new();
        if(found)
        {
            PathNode currentPath = EndNode;
			this.path.Add(currentPath);
            while( currentPath != StartNode)
            {
                currentPath = path[currentPath];
			    this.path.Add(currentPath);
            }
		    this.path.Reverse();
            CreateLine();

        }

		   
	}

    private void CreateLine()
    {
        foreach (var item in Spheres)
        {
            Destroy(item.gameObject);
        }

        Spheres.Clear();
		
		for (int i = 0; i < ShortestPath.Count-1; i++)
		{
            Vector3 dir = ShortestPath[i+1].transform.position - ShortestPath[i].transform.position;

            int dotCount = Mathf.FloorToInt(dir.magnitude / dotSpacing);
            if( i == ShortestPath.Count-2 ) { dotCount++; }
            for(int x = 0; x < dotCount; x++)
            {
                var sphere = Instantiate(Sphere, 
                    ShortestPath[i].transform.position + LineOffset + x*dotSpacing * dir.normalized, 
                    Quaternion.identity,transform);
                sphere.SetMaterials(new List<Material>() { material });
                Spheres.Add(sphere);
            }
		}
	}

    float GetDistance(PathNode Start, PathNode End)
    {
        return Vector3.Distance(Start.transform.position, End.transform.position);
    }

    public PathNode GetNextNode(PathNode currentNode)
    {
        if(path == null || path.Count == 0)  return null;

        int index = path.FindIndex(e => e == currentNode);

        if(index == -1 || index == path.Count - 1) 
            return null;
        return path[index + 1];
    }

	public PathNode GetPreviousNode(PathNode currentNode)
	{
		if (path == null || path.Count == 0) return null;

		int index = path.FindIndex(e => e == currentNode);

		if (index == -1 || index == path.Count - 1 || index == 0)
			return null;
		return path[index - 1];
	}

	public PathNode GetStartNode()
    {
		if (path == null || path.Count == 0) return null;
		return path[0];
    }

}
