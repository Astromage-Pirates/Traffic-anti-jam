using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinSpeedSign : TrafficSign
{
    [SerializeField]
    private int minSpeed;
    public int MinSpeed => minSpeed;
}
