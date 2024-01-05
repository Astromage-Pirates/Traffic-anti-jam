using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BetterPath : MonoBehaviour
{

    [field: SerializeField]
    public PathNode StartNode { get; private set; }

    [field: SerializeField]
    public PathNode EndNode { get; private set; }

    
    private List<PathNode> path;

    public List<PathNode> ShortestPath => path;


	private void Start()
	{
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
        PathNode currentPath = EndNode;
		this.path = new()
		{
			currentPath
		};
        while(found && currentPath != StartNode)
        {
            currentPath = path[currentPath];
			this.path.Add(currentPath);
        }
		this.path.Reverse();

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
