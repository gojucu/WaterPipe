using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool isExplored = false; // ok as is a data class
    public Waypoint exploredFrom;

    Vector2Int gridPos;

    const int gridSize = 1;

    public Vector3 GetGridPos()
    {
        return new Vector3
        (
        Mathf.RoundToInt(transform.position.x / gridSize),
        Mathf.RoundToInt(transform.position.y / gridSize)
        );
    }
}
