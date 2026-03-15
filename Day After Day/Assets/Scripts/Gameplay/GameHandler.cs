using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameHandler : MonoBehaviour
{
    public LoopHandler loopHandler;

    public int score;
    public int money = 2000;

    public int totalLoops = 6;
    private int currentLoop = 0;

    public bool next = false;

    void Start()
    {
        score = 0;
        loopHandler = GetComponent<LoopHandler>();
        StartCoroutine(RunGameLoops());
    }

    IEnumerator RunGameLoops()
    {
        while (currentLoop < totalLoops)
        {
            if (currentLoop == 1)
            {
                //TDODO: TELL THE USER THAT THE CLOSEST PANTRY IS VANDALIZED
                PantryClass closedPantry = GameObject.Find("Pantry (1)").GetComponent<PantryClass>();
                closedPantry.Jobable = false;
                closedPantry.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (currentLoop == 3)
            {
                //TODO: PANTRIES FOOD *.8
                //TODO: TELL PLAYER WEATHER HURT FOOD QUALITY
                GameObject[] pantries = GameObject.FindGameObjectsWithTag("Pantry");
                foreach (GameObject pantry in pantries)
                {
                    int[] food = pantry.GetComponent<PantryClass>().food;
                    for (int i = 0; i < food.Length; i++)
                    {
                        food[i]--;
                        if (food[i] < 0) food[i] = 0;
                    }
                }

            }
            if (currentLoop == 5)
            {
                //TODO: CLOSE LARGE PANTRY
            }
            next = false;
            StartCoroutine(loopHandler.StartLoop(currentLoop, money));

            yield return new WaitUntil(() => next);

            money -= (int) loopHandler.GetWeeklyCost();

            currentLoop++;
            loopHandler.loopOver = false;
        }

        Debug.Log("All loops finished! Final money: " + money);
    }

    public void OnSwitchSceneButton()
    {
        StartCoroutine(SwitchScene());
    }

    public IEnumerator SwitchScene()
    {
        yield return StartCoroutine(loopHandler.CloseLoop());
        yield return new WaitForSeconds(0.5f);
        next = true;
    }
}