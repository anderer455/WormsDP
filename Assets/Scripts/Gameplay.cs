using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamColor
{
    Neutral = 0,
    Blue = 1,
    Red = 2,
    Green = 3,
    Yellow = 4
}

public class Gameplay : MonoBehaviour
{
    private int activePlayer = Random.Range(1, 4);


    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
