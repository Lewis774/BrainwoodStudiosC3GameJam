using UnityEngine;
using System;
using System.Linq;

public class MapMovementClass : MonoBehaviour
{
    public double playerDistanceTraveled;
    public PantryClass currentPantry;
    public int savings;
    public LoopHandler LoopHandler;
    public GameHandler gameHandler;
    public UIHandler uiHandler;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoopHandler = GameObject.Find("Game").GetComponent<LoopHandler>();
        gameHandler = GameObject.Find("Game").GetComponent<GameHandler>();
        uiHandler = GameObject.Find("UI").GetComponent<UIHandler>();

        currentPantry.gameObject.SetActive(true);
        foreach (PantryClass pantry in currentPantry.closestNodes)
        {
            pantry.gameObject.SetActive(true);
        }

        // Ensure player begins at origin
        var positions = currentPantry.transform.position;
        transform.position = new Vector2(positions.x, positions.y);

        playerDistanceTraveled = 0;

    }

    // Update is called once per frame
    void Update()
    {    
        if (UnityEngine.InputSystem.Keyboard.current.wKey.wasPressedThisFrame)
        {
            PantryClass lastPantry = currentPantry;
            moveNorth();
            //deactivateNodes(lastPantry);
            //activateNodes(currentPantry);
            
            
        }
        if (UnityEngine.InputSystem.Keyboard.current.aKey.wasPressedThisFrame)
        {
            PantryClass lastPantry = currentPantry;
            moveWest();
            //deactivateNodes(lastPantry);
            //activateNodes(currentPantry);
            
        }
        if (UnityEngine.InputSystem.Keyboard.current.sKey.wasPressedThisFrame)
        {
            PantryClass lastPantry = currentPantry;
            moveSouth();
            //deactivateNodes(lastPantry);
            //activateNodes(currentPantry);
            
        }
        if (UnityEngine.InputSystem.Keyboard.current.dKey.wasPressedThisFrame)
        {
            PantryClass lastPantry = currentPantry;
            moveEast();
            //deactivateNodes(lastPantry);
            //activateNodes(currentPantry);
            
        }
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
        activateNodes(currentPantry);
        if (currentPantry.tag == "Job" && currentPantry.Jobable)
        {
            currentPantry.Jobable = false;
            int addedTime = UnityEngine.Random.Range(1, 3);
            int moneyChance = UnityEngine.Random.Range(1, 100);
            if (moneyChance < 50)
            {
                gameHandler.money += 25;
            }
            else if (moneyChance < 83)
            {
                gameHandler.money += 50;
            }
            else
            {
                gameHandler.money += 75;
            }

            //TODO: OVERLAY WORK SCRIPT
            LoopHandler.time += addedTime;
            uiHandler.UpdateMoney(gameHandler.money);
        }
        if (currentPantry.tag == "Pantry")
        {
            takeFood(currentPantry);
            LoopHandler.Collected();
        }
    }

    void deactivateNodes(PantryClass pantry)
    {
        foreach (PantryClass p in pantry.closestNodes)
        {
            p.gameObject.SetActive(false);
        }
    }
    void activateNodes(PantryClass pantry)
    {
        foreach (PantryClass p in pantry.closestNodes)
        {
            p.gameObject.SetActive(true);
        }
    }
    void moveNorth()
    {
        PantryClass nextPantry = null;
            double maxY = -1e6;
            foreach (PantryClass pantry in currentPantry.closestNodes)
            {
                if (pantry.gameObject.transform.position.y > maxY)
                {
                    maxY = pantry.gameObject.transform.position.y;
                    nextPantry = pantry;
                }
            }
            var positions = nextPantry.transform.position;
            float distance = (float) Math.Sqrt(Math.Pow(transform.position.x-positions.x, 2)
                                            + Math.Pow(transform.position.y-positions.y, 2));
            float TravelTime = (distance * 100) % 10;
            currentPantry = nextPantry;
            interact(currentPantry);
                    

            // move the player character to the new node
            playerDistanceTraveled += TravelTime;
            LoopHandler.time += TravelTime;
            transform.position = new Vector2(positions.x, positions.y);
    }
    void moveSouth()
    {
        PantryClass nextPantry = null;
        double minY = Mathf.Infinity;
        foreach (PantryClass pantry in currentPantry.closestNodes)
        {
            if (pantry.gameObject.transform.position.y < minY)
            {
                minY = pantry.gameObject.transform.position.y;
                nextPantry = pantry;
            }
        }
        var positions = nextPantry.transform.position;
        float distance = (float) Math.Sqrt(Math.Pow(transform.position.x-positions.x, 2)
                                        + Math.Pow(transform.position.y-positions.y, 2));
        float TravelTime = (distance * 100) % 10;
        currentPantry = nextPantry;
        interact(currentPantry);
                    

        // move the player character to the new node
        playerDistanceTraveled += TravelTime;
        LoopHandler.time += TravelTime;
        transform.position = new Vector2(positions.x, positions.y);
    }
    void moveEast()
    {
        PantryClass nextPantry = null;
        double maxX = -1e6;
        foreach (PantryClass pantry in currentPantry.closestNodes)
        {
            if (pantry.gameObject.transform.position.x > maxX)
            {
                maxX = pantry.gameObject.transform.position.x;
                nextPantry = pantry;
            }
        }
        var positions = nextPantry.transform.position;
        float distance = (float) Math.Sqrt(Math.Pow(transform.position.x-positions.x, 2)
                                        + Math.Pow(transform.position.y-positions.y, 2));
        float TravelTime = (distance * 100) % 10;
        currentPantry = nextPantry;
        interact(currentPantry);
                    

        // move the player character to the new node
        playerDistanceTraveled += TravelTime;
        LoopHandler.time += TravelTime;
        transform.position = new Vector2(positions.x, positions.y);
    }
    void moveWest()
    {
        PantryClass nextPantry = null;
        double minX = Mathf.Infinity;
        foreach (PantryClass pantry in currentPantry.closestNodes)
        {
            if (pantry.gameObject.transform.position.x < minX)
            {
                minX = pantry.gameObject.transform.position.x;
                nextPantry = pantry;
            }
        }
        var positions = nextPantry.transform.position;
        float distance = (float) Math.Sqrt(Math.Pow(transform.position.x-positions.x, 2)
                                        + Math.Pow(transform.position.y-positions.y, 2));
        float TravelTime = (distance * 100) % 10;
        currentPantry = nextPantry;
        interact(currentPantry);
                    

        // move the player character to the new node
        playerDistanceTraveled += TravelTime;
        LoopHandler.time += TravelTime;
        transform.position = new Vector2(positions.x, positions.y);
    }

    
}
