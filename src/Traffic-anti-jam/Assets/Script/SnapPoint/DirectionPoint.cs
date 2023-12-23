using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionPoint : MonoBehaviour
{
    public enum Direction
    {
        forward = 0,
        backward = 180,
        left = 90,
        right = -90
    }

    public Direction direction;
}
