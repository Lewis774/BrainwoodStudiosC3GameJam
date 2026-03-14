using UnityEngine;
using System;
using System.Linq;

public class MapMovementClass : MonoBehaviour
{
    public int[] foodGathered;
    public double playerDistanceTraveled;
    public double timeLimit;
    public PantryClass currentPantry;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure player begins at origin
        var pos = transform.position;
        pos.x = 0;
        pos.y = 0;

        foodGathered = new int[5];

        // TODO: Remove hard coding and add limiting factor
        timeLimit = 100;
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
                double distance = Math.Sqrt(Math.Pow(transform.position.x-positions.x, 2)
                                            + Math.Pow(transform.position.y-positions.y, 2));
                if (playerDistanceTraveled + distance > timeLimit)
                {
                    print("Not enough time!!!");
                    return;
                } 
                else if (currentPantry.closestNodes.Contains(nextPantry))
                {
                    currentPantry = nextPantry;
                    takeFood(currentPantry);

                    // move the player character to the new node
                    playerDistanceTraveled += distance;
                    transform.position = new Vector2(positions.x, positions.y);
                } 
                else 
                {
                    print("Not CLose Enough!!!"); 
                }// if else

            } // if hit collider
        } // if input mouse down
    } // Update()

    void takeFood(PantryClass currentPantry)
    {
        for (int i = 0; i < currentPantry.food.Length; i++)
        {
            foodGathered[i] += currentPantry.food[i];
            currentPantry.food[i] = 0;
        }
        
    }

    
}
