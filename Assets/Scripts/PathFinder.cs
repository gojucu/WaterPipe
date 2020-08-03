using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Waypoint startWaypoint, endWaypoint;

    Dictionary<Vector3, Waypoint> grid = new Dictionary<Vector3, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>();
    bool isRunning = true;
    Waypoint searchCenter;
    Waypoint backSearch;
    List<Waypoint> path = new List<Waypoint>();
    List<Vector3Int> directions = new List<Vector3Int>();
    List<Vector3Int> directionsBack = new List<Vector3Int>();

    //Vector3Int[] directions =
    //{
    //    Vector3Int.up,
    //    Vector3Int.right,
    //    Vector3Int.down,
    //    Vector3Int.left
    //};
    void Start()
    {
        GetPath();

    }
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
                Debug.LogWarning("Overlapping block" + waypoint);
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
        if(searchCenter != startWaypoint) {
            List<Vector3Int> directions = FindPipeDirections();
        }
        else
        {
            directions.Add(Vector3Int.up);
            directions.Add(Vector3Int.right);
            directions.Add(Vector3Int.down);
            directions.Add(Vector3Int.left);
        }
        foreach(var dir in directions)
        {
            Debug.Log(dir+searchCenter.name);
        }

        foreach (Vector3 direction in directions)//ilk foreach searchCenter yani keşfeden waypointe ait yönler
        {
            Vector3 neighbourCoordinates = searchCenter.GetGridPos() + direction;
            
            var waypoints = FindObjectsOfType<Waypoint>();
            foreach(var wap in waypoints) //neighbour un waypointini bulmakiçin
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
                            Debug.Log("Arkayı Görüyor.");
                        }
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
        var pipeDir = backSearch.transform.Find("Pipe");
        PipeRotater d = pipeDir.GetComponent<PipeRotater>();
        var gate = d.pipeGates;

        directionsBack.Clear();

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
            isRunning = false;
        }
        //if ("hasWater" == "hasWater")
        //{
        //    Debug.Log("Water reached its goal");
        //    //pipeRotater.hasWater = true;
        //}
    }
}
