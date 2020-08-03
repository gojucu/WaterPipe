using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
//[RequireComponent(typeof(Waypoint))]

public class PipeEditor : MonoBehaviour
{

    //place
    Vector2Int gridPos;

    const int gridSize = 1;

    public int GetGridSize()
    {
        return gridSize;
    }

    public Vector3 GetGridPos()
    {
        return new Vector3
        (
        Mathf.RoundToInt(transform.position.x / gridSize),
        Mathf.RoundToInt(transform.position.y / gridSize)
        );
    }
    //Waypoint waypoint;

    //private void Awake()
    //{
    //    waypoint = GetComponent<Waypoint>();
    //}


    void Update()
    {
        SnapToGrid();
        UpdateLabel();
    }
    private void SnapToGrid()
    {
        int gridSize = GetGridSize();
        transform.position = new Vector3(
            GetGridPos().x * gridSize,
            GetGridPos().y * gridSize,
            0f
            );
    }
    public void UpdateLabel()
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        string labelText = GetGridPos().x + "," + GetGridPos().y;
        textMesh.text = labelText;
        //gameObject.name = labelText;
    }

 }
