using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class NodeHandler : MonoBehaviour
{
    public GameObject nodePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void DeleteAllCurrentNodes()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");

        foreach (GameObject node in nodes)
        {
            if (Application.isPlaying)
                Destroy(node);
            else
                DestroyImmediate(node);
        }
    }

    public void LoadNodesFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "nodes.json");

        if (!File.Exists(path))
        {
            Debug.LogError("JSON file not found: " + path);
            return;
        }

        string json = File.ReadAllText(path);

        NodeDataList wrapper = JsonUtility.FromJson<NodeDataList>(json);

        foreach (NodeData node in wrapper.nodes)
        {
            GameObject newNode;

            if (nodePrefab != null)
                newNode = Instantiate(nodePrefab);
            else
                newNode = new GameObject();

            if (node.ID == 0)
                newNode.name = "Node";
            else
                newNode.name = "Node " + node.ID;

            newNode.tag = "Node";

            newNode.transform.position = new Vector3(
                node.location[0],
                node.location[1],
                node.location[2]
            );
        }

        Debug.Log("Nodes regenerated from JSON.");
    }

    public void GenerateNodeData()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");

        Debug.Log(nodes.Length);

        List<NodeData> nodeList = new List<NodeData>();

        foreach (GameObject node in nodes)
        {
            NodeData data = new NodeData();

            data.ID = ExtractID(node.name);

            Vector3 pos = node.transform.position;
            data.location = new float[] { pos.x, pos.y, pos.z };

            data.Connections = new int[0];

            nodeList.Add(data);
        }

        NodeDataList wrapper = new NodeDataList();
        wrapper.nodes = nodeList;

        string json = JsonUtility.ToJson(wrapper, true);
        string path = Path.Combine(Application.persistentDataPath, "nodes.json");

        File.WriteAllText(path, json);

        Debug.Log("Saved JSON to: " + path);
    }

    int ExtractID(string nodeName)
    {
        if (nodeName == "Node")
            return 0;

        int start = nodeName.IndexOf("(") + 1;
        int end = nodeName.IndexOf(")");

        if (start > 0 && end > start)
        {
            string idStr = nodeName.Substring(start, end - start);
            return int.Parse(idStr);
        }

        return -1;
    }
}

[System.Serializable]
public class NodeData
{
    public int ID;
    public float[] location;
    public int[] Connections;
}

[System.Serializable]
public class NodeDataList
{
    public List<NodeData> nodes;
}
