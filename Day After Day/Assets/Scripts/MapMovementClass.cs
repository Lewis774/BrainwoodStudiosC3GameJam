using UnityEngine;
using System;
using System.Linq;

public class MapMovementClass : MonoBehaviour
{
    public double playerDistanceTraveled;
    public PantryClass currentPantry;
    public int savings;
    public LoopHandler LoopHandler;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoopHandler = GameObject.Find("Game").GetComponent<LoopHandler>();
        // Ensure player begins at origin
        var pos = transform.position;
        pos.x = currentPantry.gameObject.transform.position.x;
        pos.y = currentPantry.gameObject.transform.position.y;

        playerDistanceTraveled = 0;

    }

    // Update is called once per frame
    void Update()
    {
        // identify mouse clicks
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null) {

                // identify current pantry and verify proximity
                var nextPantry = hit.collider.gameObject.GetComponent<PantryClass>();
                var positions = nextPantry.transform.position;
                float distance = (float) Math.Sqrt(Math.Pow(transform.position.x-positions.x, 2)
                                            + Math.Pow(transform.position.y-positions.y, 2));
                float TravelTime = (distance * 100) % 10;

                // ensure the player has the time to reach this node
                if (playerDistanceTraveled + TravelTime > LoopHandler.time)
                {
                    bool playable = false;
                    foreach (PantryClass pantry in currentPantry.closestNodes)
                    {
                        if (!pantry) break;
                        var pos = pantry.transform.position;
                        double dis = Math.Sqrt(Math.Pow(transform.position.x-pos.x, 2)
                                            + Math.Pow(transform.position.y-pos.y, 2));
                        double trav = (distance * 100) % 10;
                        if (playerDistanceTraveled + trav < LoopHandler.time) playable = true;
                    }
                    if (!playable)
                    {
                        //TODO: CYCLE MUST BE ENDED!!!!
                    }
                    return;
                } 
                // ensure the next node is within a reasonable distance
                else if (currentPantry.closestNodes.Contains(nextPantry))
                {
                    currentPantry = nextPantry;
                    interact(currentPantry);
                    

                    // move the player character to the new node
                    playerDistanceTraveled += TravelTime;
                    LoopHandler.time += TravelTime;
                    transform.position = new Vector2(positions.x, positions.y);
                } // if else
            } // if hit collider
        } // if input mouse down
    } // Update()

    void takeFood(PantryClass currentPantry)
    {
        for (int i = 0; i < currentPantry.food.Length; i++)
        {
            Debug.Log(currentPantry.food[i]);
            LoopHandler.foodGathered[i] += currentPantry.food[i];
            currentPantry.food[i] = 0;
        }
        
    }

    public void interact(PantryClass currentPantry)
    {
        if (currentPantry.tag == "Job" && currentPantry.Jobable)
        {
            currentPantry.Jobable = false;
            int JobChances = UnityEngine.Random.Range(0,100);
            if (JobChances < 3)
            {
                //TODO: Replace with more robust response
                Debug.Log("You got the job!");
            }
            else
            {
                //TODO: Replace with more robust response
                Debug.Log("Sorry, We've decided to go with a more promising candidate.");
            }
        }
        if (currentPantry.tag == "Pantry")
        {
            takeFood(currentPantry);
            LoopHandler.Collected();
        }
    }

    
}
