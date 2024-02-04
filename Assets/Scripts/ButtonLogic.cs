using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    private delegate void ChangeState();
    private ChangeState onPress;
    private ChangeState onDepress;
    private int count;
    private GameState gameState;

    [SerializeField] private GameObject baseModel;
    public enum Functionality { OpenGate, UpdateGameState };
    [SerializeField] private Functionality buttonFunctionality;

    private void OpenGateFunction()
    {
        Debug.Log("Opening the gate.");
    }

    private void CloseGateFunction()
    {
        Debug.Log("Closing the gate.");
    }

    private void IncrementFunction()
    {
        gameState.IncrementScore();
    }

    private void DecrementFunction()
    {
        gameState.DecrementScore();
    }

    void Start()
    {
        this.count = 0;
        this.gameState = GameObject.Find("GameState").GetComponent<GameState>();
        this.baseModel.GetComponent<MeshRenderer>().material.color = Color.red;
        if (this.buttonFunctionality == Functionality.OpenGate)
        {
            this.onPress = this.OpenGateFunction;
            this.onDepress = this.CloseGateFunction;
        }
        else
        {
            this.onPress = this.IncrementFunction;
            this.onDepress = this.DecrementFunction;
        }
    }

    public void OnEnter()
    {
        this.count++;
        if (this.count == 1)
        {
            this.baseModel.GetComponent<MeshRenderer>().material.color = Color.green;
            onPress();
        }
        Debug.Log("Enter");
    }

    public void OnExit()
    {
        this.count--;
        if (this.count == 0)
        {
            this.baseModel.GetComponent<MeshRenderer>().material.color = Color.red;
            onDepress();
        }
        Debug.Log("Exit");
    }
}
