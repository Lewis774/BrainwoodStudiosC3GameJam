using UnityEngine;
using System;

public class PantryClass : MonoBehaviour
{
   
    public int[] food;
    public PantryClass[] closestNodes;
    public bool Jobable;

    public LoopHandler loopHandler;
    public UIHandler uiHandler;
    public GameHandler gameHandler;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loopHandler = GameObject.Find("Game").GetComponent<LoopHandler>();
        uiHandler = GameObject.Find("UI").GetComponent<UIHandler>();
        gameHandler = GameObject.Find("Game").GetComponent<GameHandler>();

        var allNodes = FindObjectsByType<PantryClass>(FindObjectsSortMode.None);

        closestNodes = new PantryClass[5];
        for (int i = 0; i < closestNodes.Length; i++)
        {
            closestNodes[i] = getClosestNode(allNodes);
        }
        food = new int[4];
        
        // Not a node and therefore a pantry
        if (tag == "Pantry")
        {
            for (int i = 0; i < food.Length; i++)
            {
                food[i] = UnityEngine.Random.Range(0, 2) * (i + 1);
            }
        }
        

        Jobable = true;
        
    
    
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
