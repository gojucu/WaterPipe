using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterSource : MonoBehaviour
{
    //Rotate
    public float RotateSpeed = 600f;
    private Quaternion currentRotation;
    private Quaternion targetRotation;
    //Next level panel
    public GameObject EndPanel;

    void OnMouseDown()
    {
        PathFinder pathfinder = FindObjectOfType<PathFinder>();

        ClearThings(pathfinder);

        pathfinder.GetPath();
        if (pathfinder.isEndReached)
        {
            SpinValve();
            EndPanel.SetActive(true);
            Debug.Log(SceneManager.GetActiveScene().name);
            Debug.Log("son bulundu");
        }
        else
        {
            Debug.Log("Bulunamadı");
        }

    }
    void Start()
    {
        var valve = transform.Find("ValvePipe").Find("Valve");
        Debug.Log(valve.name);
        currentRotation = valve.rotation;
        targetRotation = valve.rotation;

        EndPanel.SetActive(false);//panel başta gözükmüyor
    }
    private void SpinValve()
    {

        if (currentRotation == targetRotation)
        {
            targetRotation *= Quaternion.Euler(0.0f, -180.0f, 0f);

        }
    }
    void Update()
    {
        currentRotation = Quaternion.RotateTowards(transform.Find("ValvePipe").Find("Valve").rotation, targetRotation, RotateSpeed * Time.deltaTime);

        transform.Find("ValvePipe").Find("Valve").rotation = currentRotation;
    }

    private static void ClearThings(PathFinder pathfinder)//Önceki aramadan kalan bilgileri temizle
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
