using UnityEngine;
using System;
using Unity.VisualScripting;
using TMPro;
using UnityEditorInternal;

public class MapMovementClass : MonoBehaviour
{
    public double playerDistanceTraveled;
    public PantryClass currentPantry;
    public LoopHandler LoopHandler;
    public GameHandler gameHandler;
    public UIHandler uiHandler;
    public GameObject home;

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

        var positions = currentPantry.transform.position;
        transform.position = new Vector2(positions.x, positions.y);

        playerDistanceTraveled = 0;

    }

    // Update is called once per frame
    void Update()
    {    
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                PantryClass pantry = hit.collider.GetComponent<PantryClass>();

                if (pantry != null)
                {
                    if (pantry.tag == "Job")
                    {
                        uiHandler.UpdateInfoPanel("Type: Job\n" + 
                                                  "Time: " + pantry.time + " min\n" + 
                                                  "Worked: " + !pantry.interactable + "\n" + 
                                                  "Pay: " + pantry.money);
                    }
                    else if (pantry.tag == "Pantry" || pantry.tag == "LargePantry")
                    {
                        string message = "";
                        if (pantry.tag == "Pantry")
                        {
                            message = "Type: Outside Pantry\n" + "Collected: " + !pantry.interactable + "\n";
                        }
                        if (pantry.tag == "LargePantry")
                        {
                            message = "Type: Large Pantry\n" + "Collected: " + !pantry.interactable + "\n";
                        }
                        if (pantry.food[0] != 0)
                        {
                           message += "Protein x" + pantry.food[0] + "\n";
                        }
                        if (pantry.food[1] != 0)
                        {
                           message += "Vegetable x" + pantry.food[1] + "\n";
                        }
                        if (pantry.food[2] != 0)
                        {
                           message += "Carbs x" + pantry.food[2] + "\n";
                        }
                        if (pantry.food[3] != 0)
                        {
                           message += "Fruits x" + pantry.food[3] + "\n";
                        }
                        if (pantry.food[4] != 0)
                        {
                           message += "Dairy x" + pantry.food[4] + "\n";
                        }
                        uiHandler.UpdateInfoPanel(message);
                    }
                }
            }
        }

        if (UnityEngine.InputSystem.Keyboard.current.wKey.wasPressedThisFrame)
        {
            PantryClass lastPantry = currentPantry;
            if (!LoopHandler.loopOver) moveNorth();
            //deactivateNodes(lastPantry);
            //activateNodes(currentPantry);
            
            
        }
        if (UnityEngine.InputSystem.Keyboard.current.aKey.wasPressedThisFrame)
        {
            PantryClass lastPantry = currentPantry;
            if (!LoopHandler.loopOver) moveWest();
            //deactivateNodes(lastPantry);
            //activateNodes(currentPantry);
            
        }
        if (UnityEngine.InputSystem.Keyboard.current.sKey.wasPressedThisFrame)
        {
            PantryClass lastPantry = currentPantry;
            if (!LoopHandler.loopOver) moveSouth();
            //deactivateNodes(lastPantry);
            //activateNodes(currentPantry);
            
        }
        if (UnityEngine.InputSystem.Keyboard.current.dKey.wasPressedThisFrame)
        {
            PantryClass lastPantry = currentPantry;
            if (!LoopHandler.loopOver) moveEast();
            //deactivateNodes(lastPantry);
            //activateNodes(currentPantry);
            
        }
    } 

    public void determineNode(PantryClass currentPantry)
    {
        activateNodes(currentPantry);
        if (currentPantry.tag == "Job" && currentPantry.interactable)
        {
            uiHandler.interactButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Work";
            uiHandler.interactButton.SetActive(true);
            currentPantry.interactable = false;
            // uiHandler.UpdateInfoPanel(addedTime, "Money Available: $" + workMoney);
        }
        else if (currentPantry.tag == "Pantry" || currentPantry.tag == "LargePantry")
        {            
            uiHandler.interactButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Collect";
            uiHandler.interactButton.SetActive(true);
            uiHandler.UpdateGameLog("Visited Pantry!");
        }
        else
        {
            uiHandler.interactButton.SetActive(false);
        }
    }

    public void interact()
    {
        Debug.Log("Run");
        if (currentPantry.tag == "Job")
        {
            LoopHandler.time += currentPantry.time;
            gameHandler.money += currentPantry.money;
            uiHandler.UpdateMoney(gameHandler.money);
            uiHandler.UpdateGameLog("Worked for $" + currentPantry.money + "!");
        }
        if (currentPantry.tag == "Pantry" || currentPantry.tag == "LargePantry")
        {
            for (int i = 0; i < currentPantry.food.Length; i++)
            {
                LoopHandler.foodGathered[i] += currentPantry.food[i];
                currentPantry.food[i] = 0;
            }    
            uiHandler.UpdateGameLog("Collected food!");
            LoopHandler.Collected();
        }
        currentPantry.interactable = false;
        uiHandler.interactButton.SetActive(false);
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
        determineNode(currentPantry);
                
        // move the player character to the new node
        playerDistanceTraveled += TravelTime;
        LoopHandler.time += TravelTime;
        transform.position = new Vector2(positions.x, positions.y);
        uiHandler.UpdateGameLog("Moved for " + (int) TravelTime + " min ");
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
        determineNode(currentPantry);

        // move the player character to the new node
        playerDistanceTraveled += TravelTime;
        LoopHandler.time += TravelTime;
        transform.position = new Vector2(positions.x, positions.y);
        uiHandler.UpdateGameLog("Moved for " + (int) TravelTime + " min ");        
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
        determineNode(currentPantry);
                    
        // move the player character to the new node
        playerDistanceTraveled += TravelTime;
        LoopHandler.time += TravelTime;
        transform.position = new Vector2(positions.x, positions.y);
        uiHandler.UpdateGameLog("Moved for " + (int) TravelTime + " min ");
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
        determineNode(currentPantry);     

        // move the player character to the new node
        playerDistanceTraveled += TravelTime;
        LoopHandler.time += TravelTime;
        transform.position = new Vector2(positions.x, positions.y);
        uiHandler.UpdateGameLog("Moved for " + (int) TravelTime + " min ");
    } 
}
