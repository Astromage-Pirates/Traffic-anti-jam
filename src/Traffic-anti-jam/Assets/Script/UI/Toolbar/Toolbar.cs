using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Toolbar : MonoBehaviour
{
    private float targetX = 0;
    private void FixedUpdate()
    {
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, targetX, 10f * Time.deltaTime),
            transform.position.y,
            0.0f
        );
        // if (Mathf.Abs(targetX - transform.position.x) == 0)
        // {
        //     targetX = 0;
        // }
    }
    public void ShowToolbar()
    {
        targetX = transform.position.x - 170;
    }
}
