using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelEditor : EditorWindow
{

    int index;
    int[] zIndex= { 0, -90, 180, 90 };
    string[] directions = { "up", "right", "down", "left" };

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditor>("Level Editor");
    }
    void OnGUI()
    {
        GUILayout.Label("Choose pipe to add", EditorStyles.boldLabel);

        //myString = EditorGUILayout.TextField("Name", myString);

        if (GUILayout.Button("Add -Pipe"))
        {
            InstantiatePipe("-PipeBox");
        }
        if (GUILayout.Button("Add LPipe"))
        {
            InstantiatePipe("LPipeBox");
        }
        if (GUILayout.Button("Rotate Selected"))
        {
            RotateSelected();
        }
        //if (GUILayout.Button("New Scene"))
        //{

        //    //int sceneCount = SceneManager.sceneCountInBuildSettings + 1;
        //    //FileUtil.CopyFileOrDirectory("Assets/Scenes/SampleScene.unity", "Assets/Scenes/Chapter"+sceneCount+".unity");
        //    //AssetDatabase.Refresh();

        //}
    }

    private static void RotateSelected()
    {
        var hey = Selection.activeTransform.gameObject;
        PipeRotater pipeRotater = hey.transform.Find("Pipe").GetComponent<PipeRotater>();
        //var pipeDirection = pipeRotater.strDirection;
        if (pipeRotater.strDirection == "up") pipeRotater.strDirection = "right";
        else if (pipeRotater.strDirection == "right") pipeRotater.strDirection = "down";
        else if (pipeRotater.strDirection == "down") pipeRotater.strDirection = "left";
        else if (pipeRotater.strDirection == "left") pipeRotater.strDirection = "up";

        hey.transform.Find("Pipe").rotation *= Quaternion.Euler(0.0f, 0.0f, -90.0f);
        //Debug.Log(hey.name);
    }

    void InstantiatePipe(string pipeType)
    {
        string path = $"Assets/Prefabs/{pipeType}.prefab";
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));

        GameObject pipe = Instantiate(gameObject);
        pipe.transform.parent = GameObject.Find("World").transform;

        pipe.transform.position = new Vector3(-6, 1, 0);

        index = Random.Range(0, zIndex.Length);
        Debug.Log(zIndex[index]);
        pipe.transform.Find("Pipe").rotation = Quaternion.Euler(0, 0, zIndex[index]);
        PipeRotater pipeRotater = pipe.transform.Find("Pipe").GetComponent<PipeRotater>();
        pipeRotater.strDirection = directions[index];
    }
}
