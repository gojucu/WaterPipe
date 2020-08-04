using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeRotater : MonoBehaviour
{
    //CheckWater
    [Header("PipeVariables")]
    public bool hasWater = false;
    //Rotate
    public float RotateSpeed = 600f;
    private Quaternion currentRotation;
    private Quaternion targetRotation;
    //Pipe Directions
    public string pipeGates;
    public string strDirection;

    
    [SerializeField] GameObject pipePrefab;


    void Start()
    {
        currentRotation = transform.rotation;
        targetRotation = transform.rotation;
        FindDirections();
    }

    private void FindDirections()
    {
        string type = "";
        type = pipePrefab.name;
        if(type=="LPipe")
        {
            if (strDirection == "up")
            {
                pipeGates = "1100";
            }
            if (strDirection == "right")
            {
                pipeGates = "0110";
            }
            if (strDirection == "down")
            {
                pipeGates = "0011";
            }
            if (strDirection == "left")
            {
                pipeGates = "1001";
            }
        }
        if (type == "-Pipe")
        {
            if (strDirection == "up")
            {
                pipeGates = "1010";
            }
            if (strDirection == "right")
            {
                pipeGates = "0101";

            }
            if (strDirection == "down")
            {
                pipeGates = "1010";
            }
            if (strDirection == "left")
            {
                pipeGates = "0101";
            }
        }
    }

    void OnMouseDown()
    {
      
        HapticFeedback();//isWorking?

        PathFinder pathfinder = FindObjectOfType<PathFinder>();
        if (currentRotation == targetRotation&& !pathfinder.isEndReached)
        {
            targetRotation *= Quaternion.Euler(0.0f, 0.0f, -90.0f);

            if (strDirection == "up") strDirection = "right";
            else if (strDirection == "right") strDirection = "down";
            else if (strDirection == "down") strDirection = "left";
            else if (strDirection == "left") strDirection = "up";

        }

    }

    void Update()
    {

        currentRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);

        transform.rotation = currentRotation;
        FindDirections();
    }

    ////////////HAPTIC//////////////
     private class HapticFeedbackManager
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private int HapticFeedbackConstantsKey;
        private AndroidJavaObject UnityPlayer;
#endif

        public HapticFeedbackManager()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
            HapticFeedbackConstantsKey=new AndroidJavaClass("android.view.HapticFeedbackConstants").GetStatic<int>("VIRTUAL_KEY");
            UnityPlayer=new AndroidJavaClass ("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
            //Alternative way to get the UnityPlayer:
            //int content=new AndroidJavaClass("android.R$id").GetStatic<int>("content");
            //new AndroidJavaClass ("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("findViewById",content).Call<AndroidJavaObject>("getChildAt",0);
#endif
    }

    public bool Execute()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
            return UnityPlayer.Call<bool> ("performHapticFeedback",HapticFeedbackConstantsKey);
#endif
        return false;
    }
}

//Cache the Manager for performance
private static HapticFeedbackManager mHapticFeedbackManager;

public static bool HapticFeedback()
{
    if (mHapticFeedbackManager == null)
    {
        mHapticFeedbackManager = new HapticFeedbackManager();
    }
    return mHapticFeedbackManager.Execute();
}
}
