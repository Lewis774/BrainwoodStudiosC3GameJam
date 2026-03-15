using System.Collections;
using UnityEngine;

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
            next = false;

            StartCoroutine(loopHandler.StartLoop(currentLoop, money));

            yield return new WaitUntil(() => next);

            money -= (int) loopHandler.GetWeeklyCost();

            if ((currentLoop + 1) % 2 == 0)
            {
                // yield return loopHandler.PlayLoopAnimation();
            }

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