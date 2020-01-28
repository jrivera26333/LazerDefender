using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    int score = 0;

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int numberGameSession = FindObjectsOfType(GetType()).Length; //How many GameSessions do we have

        if (numberGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
            DontDestroyOnLoad(gameObject);
    }

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int scoreValue)
    {
        score += scoreValue;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
