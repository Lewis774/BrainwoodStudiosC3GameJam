using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpNoise : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}