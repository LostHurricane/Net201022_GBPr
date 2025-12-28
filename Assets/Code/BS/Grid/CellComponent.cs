using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellComponent : MonoBehaviour
{
    public Vector2Int Coordinate { get => _coordinate; set => _coordinate = value; }
    private Vector2Int _coordinate;

    public void Configure(Vector2Int coordinate)
    {
        _coordinate = coordinate;
    }
}
