using UnityEngine;
using System;

public class PantryClass : MonoBehaviour
{
   
    public int[] food;
    public PantryClass[] closestNodes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var allNodes = FindObjectsByType<PantryClass>(FindObjectsSortMode.None);

        closestNodes = new PantryClass[4];
        for (int i = 0; i < closestNodes.Length; i++)
        {
            closestNodes[i] = getClosestNode(allNodes);
        }
        food = new int[5];
        if (tag == "Node")
        {
            // Any value or functionality that should be unique to Nodes specifically
        } 
        else
        {
            // Not a node and therefore a pantry
            for (int i = 0; i < food.Length; i++)
            {
            food[i] = UnityEngine.Random.Range(0, 10);
            }
        }
    
    }

    // Helper method to return the closest node in an array of all nodes on the map
    PantryClass getClosestNode (PantryClass[] nodes)
    {
        PantryClass tMin = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] == this) continue;
            float dist = Vector2.Distance(nodes[i].transform.position, currentPos);
            if (dist < minDist)
            {   
                bool SkipFlag = false;
                for (int j = 0; j < closestNodes.Length; j++)
                {
                    if (nodes[i] == closestNodes[j])
                    {
                       SkipFlag = true; 
                       break;
                    }
                }
                if (!SkipFlag)
                {
                     tMin = nodes[i];
                    minDist = dist;
                }
            }
        }
        return tMin;
    } // getClosestNode
}
