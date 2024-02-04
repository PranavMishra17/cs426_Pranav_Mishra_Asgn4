using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private int score;
    private GameObject winMessage;

    void Start()
    {
        this.winMessage = GameObject.Find("NetworkManagerUI").transform.Find("WinMessage").gameObject;
        if (winMessage == null)
        {
            Debug.Log("Win message was not found.");
        } else
        {
            Debug.Log("Win message was found.");
        }
        this.score = 0;
    }

    public void IncrementScore()
    {
        this.score++;
    }

    public void DecrementScore()
    {
        this.score--;
    }

    void Update()
    {
        if (this.score == 3)
        {
            this.winMessage.SetActive(true);
        }
    }
}
