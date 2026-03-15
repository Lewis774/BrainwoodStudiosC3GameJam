using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameHandler : MonoBehaviour
{
    public LoopHandler loopHandler;
    public UIHandler uiHandler;

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
        uiHandler = GameObject.Find("UI").GetComponent<UIHandler>();
    }

    IEnumerator RunGameLoops()
    {
        while (currentLoop < totalLoops)
        {
            if (currentLoop == 1)
            {
                uiHandler.UpdateWeeklyInfo("A Pantry has been Vandalized! Pantries marked with red are no longer accessable."); 
                PantryClass closedPantry = GameObject.Find("Pantry (1)").GetComponent<PantryClass>();
                closedPantry.Jobable = false;
                closedPantry.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (currentLoop == 3)
            {
                //TODO: PANTRIES FOOD *.8
                //TODO: TELL PLAYER WEATHER HURT FOOD QUALITY
                uiHandler.UpdateWeeklyInfo("Poor Weather. Pantries now have less food.");
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
                uiHandler.UpdateWeeklyInfo("Due to neglect, one of the large pantries has closed.");
                PantryClass closedPantry = GameObject.Find("Large Pantry").GetComponent<PantryClass>();
                closedPantry.Jobable = false;
                closedPantry.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
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