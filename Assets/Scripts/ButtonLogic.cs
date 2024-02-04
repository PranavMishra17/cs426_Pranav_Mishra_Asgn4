using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    private delegate void ChangeState(GameObject obj);
    private ChangeState onPress;
    private ChangeState onDepress;
    private int count;
    private GameState gameState;

    [SerializeField] private GameObject baseModel;
    public enum Functionality { OpenGate, UpdateGameState };
    [SerializeField] private Functionality buttonFunctionality;

    private void OpenGateFunction(GameObject obj)
    {
        Debug.Log("Opening the gate.");
    }

    private void CloseGateFunction(GameObject obj)
    {
        Debug.Log("Closing the gate.");
    }

    private void IncrementFunction(GameObject obj)
    {
        if (obj.layer == LayerMask.NameToLayer("Interactables"))
        {
            gameState.IncrementScore();
        }
    }

    private void DecrementFunction(GameObject obj)
    {
        if (obj.layer == LayerMask.NameToLayer("Interactables"))
        {
            gameState.DecrementScore();
        }
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

    public void OnEnter(GameObject obj)
    {
        this.count++;
        if (this.count == 1)
        {
            this.baseModel.GetComponent<MeshRenderer>().material.color = Color.green;
            onPress(obj);
        }
    }

    public void OnExit(GameObject obj)
    {
        this.count--;
        if (this.count == 0)
        {
            this.baseModel.GetComponent<MeshRenderer>().material.color = Color.red;
            onDepress(obj);
        }
    }
}
