using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


[CustomEditor(typeof(PathNode))]
public class PathEditor : Editor
{
	[DrawGizmo(GizmoType.Selected | GizmoType.NotInSelectionHierarchy)]
	private static void drawConnection(PathNode node, GizmoType aGizmoType)
	{
		var labelStyle = new GUIStyle();
		labelStyle.normal.textColor = Color.red;
		labelStyle.alignment = TextAnchor.MiddleCenter;
		Handles.Label(node.transform.position, node.name, labelStyle);
		DrawNode(node);
	}

	private static void DrawNode(PathNode node,PathNode To = null)
	{
		Handles.color = Color.green;
		Handles.DrawWireDisc(node.transform.position, Vector3.up, node.Radius, 0.4f);
		List<Edge> edges;
		if (To == null)
		{
			edges = node.Edges;
		}
		else
		{
			edges = node.Edges.FindAll(e => e.To == To);
		}

		var labelStyle = new GUIStyle();
		labelStyle.normal.textColor = Color.red;
		labelStyle.alignment = TextAnchor.MiddleCenter;

		foreach (var edge in edges)
		{
			if (edge.To == null)
			{
				continue;
			}
			if(edge.To.Edges.Find(e => e.To == node) != null)
			{
				DrawTwoSegMentDirection(node.transform.position, edge.To.transform.position);
			}
			else
			{
				DrawDirection(node.transform.position, edge.To.transform.position);
			}
			Vector3 dir = edge.To.transform.position - node.transform.position;
			Handles.Label(node.transform.position + dir * 0.5f, edge.Weight.ToString(), labelStyle);
		}
	}

	private static void DrawTwoSegMentDirection(Vector3 start,Vector3 end)
	{
		Handles.color = Color.black;
		Vector3 dir = end - start;
		Vector3 right = Vector3.Cross(Vector3.up,dir ).normalized*0.5f;
		Vector3 mid = start + dir * 0.5f + right;
		DrawDirection(start, mid);
		DrawDirection(mid, end);
	}

	private static void DrawDirection(Vector3 start, Vector3 end)
	{
		Handles.color = Color.black;
		Vector3 dir = end - start;
		Vector3 k = Vector3.Cross(dir, Vector3.up).normalized * 0.3f;
		Handles.DrawLine(start, end);
		Vector3 mid = start + dir * 0.5f;

		Handles.DrawLine(mid, mid - dir.normalized + k);
		Handles.DrawLine(mid, mid - dir.normalized - k);
	}

	[DrawGizmo(GizmoType.Selected | GizmoType.NotInSelectionHierarchy)]
	private static void drawConnection(BetterPath route, GizmoType aGizmoType)
	{
		var labelStyle = new GUIStyle();
		labelStyle.normal.textColor = Color.red;
		labelStyle.alignment = TextAnchor.MiddleCenter;

		if (route.ShortestPath == null || route.ShortestPath.Count == 0) return;

		for (int i = 0; i < route.ShortestPath.Count - 1; i++)
		{

			Handles.Label(route.ShortestPath[i].transform.position, route.ShortestPath[i].name, labelStyle);
			DrawNode(route.ShortestPath[i], route.ShortestPath[i + 1]);
			Handles.Label(route.ShortestPath[i + 1].transform.position, route.ShortestPath[i + 1].name, labelStyle);
		}
	}
}
