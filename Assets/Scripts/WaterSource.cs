using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    void OnMouseDown()
    {
        PathFinder pathfinder = FindObjectOfType<PathFinder>();

        ClearThings(pathfinder);

        pathfinder.GetPath();
        if (pathfinder.isEndReached)
        {
            Debug.Log("son bulundu");
        }
        else
        {
            Debug.Log("Bulunamadı");
        }

    }

    private static void ClearThings(PathFinder pathfinder)
    {
        pathfinder.grid.Clear();
        pathfinder.path.Clear();
        pathfinder.directions.Clear();
        pathfinder.directionsBack.Clear();
        
        var waypoints = FindObjectsOfType<Waypoint>();
        foreach (var wap in waypoints)
        {
            wap.isExplored = false;
            wap.exploredFrom = null;
        }
    }
}
