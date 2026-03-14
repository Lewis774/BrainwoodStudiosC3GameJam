using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameHandler : MonoBehaviour
{
    private LoopHandler loopHandler;

    public int score;

    public int money = 2000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        loopHandler = GetComponent<LoopHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
