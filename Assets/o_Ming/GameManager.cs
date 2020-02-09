using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance 
    {
        get
        {
            //Create logic to create the instance
            if(_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }
            return _instance;
        } 
    }

    public int Score { get; set; }
    public bool IsDead { get; set; }

    private void Awake()
    {
        _instance = this;
        
    }

    private void Start()
    {
        Score = 10;
    }


}
