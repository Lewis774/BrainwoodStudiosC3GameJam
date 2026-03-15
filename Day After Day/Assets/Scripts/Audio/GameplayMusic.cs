using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusic : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}