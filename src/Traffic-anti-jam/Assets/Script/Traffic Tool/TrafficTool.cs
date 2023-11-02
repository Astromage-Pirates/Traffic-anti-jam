using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficTool : MonoBehaviour
{
    private void FixedUpdate() {
        FollowingMouse();
    }

    public Vector3 FollowingMouse()
    {
        transform.position = Input.mousePosition;
        return transform.position;
    }
}
