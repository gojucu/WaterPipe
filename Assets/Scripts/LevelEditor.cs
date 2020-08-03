using UnityEngine;
using UnityEditor;

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
        if (GUILayout.Button("Add +Pipe"))
        {
            InstantiatePipe("+PipeBox");
        }
        if (GUILayout.Button("Add LPipe"))
        {
            InstantiatePipe("LPipeBox");
        }
    }
    void InstantiatePipe(string pipeType)
    {
        string path = $"Assets/Prefabs/{pipeType}.prefab";
        GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));

        GameObject pipe = Instantiate(gameObject);
        pipe.transform.parent = GameObject.Find("World").transform;

        pipe.transform.position = new Vector3(0, 0, 0);

        index = Random.Range(0, zIndex.Length);
        Debug.Log(zIndex[index]);
        pipe.transform.Find("Pipe").rotation = Quaternion.Euler(0, 0, zIndex[index]);
        PipeRotater pipeRotater = pipe.transform.Find("Pipe").GetComponent<PipeRotater>();
        pipeRotater.strDirection = directions[index];
    }
}
