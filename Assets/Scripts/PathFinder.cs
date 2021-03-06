﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Waypoint startWaypoint, endWaypoint;

    public Dictionary<Vector3, Waypoint> grid = new Dictionary<Vector3, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>();
    bool isRunning = true;
    public bool isEndReached = false;
    Waypoint searchCenter;
    Waypoint backSearch;
    public List<Waypoint> path = new List<Waypoint>();
    public List<Vector3Int> directions = new List<Vector3Int>();
    public List<Vector3Int> directionsBack = new List<Vector3Int>();


    public List<Waypoint> GetPath()
    {

        if (path.Count == 0)
        {
            CalculatePath();
        }

        return path;
    }
    private void CalculatePath()
    {
        LoadBlocks();
        BreadthFirstSearch();

        Waypoint end = endWaypoint.exploredFrom;
        if (end != null)
        {
            CreatePath();
        }

    }
    private void LoadBlocks()
    {
        var waypoints = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypoints)
        {
            var gridPos = waypoint.GetGridPos();
            if (grid.ContainsKey(gridPos))
            {
                Debug.LogWarning("Overlapping pipe" + waypoint);
            }
            else
            {
                grid.Add(gridPos, waypoint);
            }

        }
    }
    private void BreadthFirstSearch()
    {
        queue.Enqueue(startWaypoint);
        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            HaltIfEndFound();
            ExploreNeighbour();
            searchCenter.isExplored = true;
        }
    }
    private void ExploreNeighbour()
    {
        if(searchCenter != startWaypoint&&searchCenter!=endWaypoint) {
            List<Vector3Int> directions = FindPipeDirections();
        }
        else
        {
            directions.Add(Vector3Int.up);
            directions.Add(Vector3Int.right);
            directions.Add(Vector3Int.down);
            directions.Add(Vector3Int.left);
        }
        //foreach(var dir in directions)
        //{
        //    Debug.Log(dir+searchCenter.name);
        //}

        foreach (Vector3 direction in directions)//ilk foreach searchCenter yani keşfeden waypointe ait yönler
        {
            Vector3 neighbourCoordinates = searchCenter.GetGridPos() + direction;

            FindExploredFrom(neighbourCoordinates);

        }
    }

    private void FindExploredFrom(Vector3 neighbourCoordinates)
    {
        var waypoints = FindObjectsOfType<Waypoint>();
        foreach (var wap in waypoints) //neighbour un waypointini bulmakiçin
        {
            if (neighbourCoordinates == wap.transform.position && !wap.isExplored) // searchCenterın keşfettiği pipe kendisini görüyormu kontrol etmek için burdan gidiliyor
            {
                backSearch = wap;
                List<Vector3Int> directionsBack = FindPipeDirectionsBackwards();
                foreach (Vector3 backDirection in directionsBack)//yeni keşfedilen pipe ın yönleri
                {
                    Vector3 exploredFromCoordinates = backSearch.GetGridPos() + backDirection;
                    if (exploredFromCoordinates == searchCenter.GetGridPos())//searchCenter ın gridini, yeni keşfedilen waypoint görüyor mu (Borunun yönleri doğrumu ?)
                    {
                        if (grid.ContainsKey(neighbourCoordinates))
                        {
                            QueueNewNeighbours(neighbourCoordinates);
                        }
                        //Debug.Log("Arkayı Görüyor.");
                    }
                }
            }
        }
    }

    public List<Vector3Int> FindPipeDirections()
    {
        var pipeDir = searchCenter.transform.Find("Pipe");
        PipeRotater d = pipeDir.GetComponent<PipeRotater>();
        var gate = d.pipeGates;

        directions.Clear();

            if (gate[0] == '1')
            {
                directions.Add(Vector3Int.up);
            }
            if (gate[1] == '1')
            {
                directions.Add(Vector3Int.right);
            }
            if (gate[2] == '1')
            {
                directions.Add(Vector3Int.down);
            }
            if (gate[3] == '1')
            {
                directions.Add(Vector3Int.left);
            }

        return directions;

    }

    public List<Vector3Int> FindPipeDirectionsBackwards()
    {


        directionsBack.Clear();

        if (backSearch == endWaypoint)
        {
            directionsBack.Add(Vector3Int.up);
            directionsBack.Add(Vector3Int.right);
            directionsBack.Add(Vector3Int.down);
            directionsBack.Add(Vector3Int.left);
        }
        else
        {
            var pipeDir = backSearch.transform.Find("Pipe");
            PipeRotater d = pipeDir.GetComponent<PipeRotater>();
            var gate = d.pipeGates;
            if (gate[0] == '1')
            {
                directionsBack.Add(Vector3Int.up);
            }
            if (gate[1] == '1')
            {
                directionsBack.Add(Vector3Int.right);
            }
            if (gate[2] == '1')
            {
                directionsBack.Add(Vector3Int.down);
            }
            if (gate[3] == '1')
            {
                directionsBack.Add(Vector3Int.left);
            }
        }


        return directionsBack;

    }


    private void QueueNewNeighbours(Vector3 neighbourCoordinates)
    {
        Waypoint neighbour = grid[neighbourCoordinates];
        if (neighbour.isExplored || queue.Contains(neighbour))
        {
            // do nothing
        }
        else
        {
            queue.Enqueue(neighbour);
            neighbour.exploredFrom = searchCenter;
        }

    }
    private void CreatePath()
    {
        SetAsPath(endWaypoint);

        Waypoint previous = endWaypoint.exploredFrom;
        while (previous != startWaypoint)
        {
            previous = previous.exploredFrom;
            SetAsPath(previous);

        }

        SetAsPath(startWaypoint);
        path.Reverse();
    }

    private void SetAsPath(Waypoint waypoint)
    {

        path.Add(waypoint);
        //waypoint.isPlaceable = false; gerekli değil galiba
    }
    private void HaltIfEndFound()
    {
        if (searchCenter == endWaypoint)
        {
            Debug.Log("Water reached its goal");
            isEndReached = true;
            isRunning = false;
        }

    }
}
