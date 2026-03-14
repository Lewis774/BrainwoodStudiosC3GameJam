using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameHandler : MonoBehaviour
{
    public int score;

    public int money = 2000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
