using UnityEngine;

public class ButtonCollider : MonoBehaviour
{
    private delegate void ChangeState();
    public enum Functionality { OpenGate, UpdateGameState };

    private ChangeState onPress;
    private ChangeState onDepress;

    private GameState gameState;
    [SerializeField] private GameObject baseModel;
    [SerializeField] private Functionality buttonFunctionality;

    void OpenGateFunction()
    {
        Debug.Log("Opening the gate.");
    }

    void CloseGateFunction()
    {
        Debug.Log("Closing the gate.");
    }

    void IncrementFunction()
    {
        gameState.IncrementScore();
        Debug.Log("Incrementing number of points.");
    }

    void DecrementFunction()
    {
        gameState.DecrementScore();
        Debug.Log("Decrement number of points.");
    }

    void Start()
    {
        this.gameState = GameObject.Find("GameState").GetComponent<GameState>();
        this.baseModel.GetComponent<MeshRenderer>().material.color = Color.red;
        if (this.buttonFunctionality == Functionality.OpenGate)
        {
            this.onPress = this.OpenGateFunction;
            this.onDepress = this.CloseGateFunction;
        } else
        {
            this.onPress = this.IncrementFunction;
            this.onDepress = this.DecrementFunction;
        }
    }

    void OnTriggerEnter()
    {
        this.baseModel.GetComponent<MeshRenderer>().material.color = Color.green;
        onPress();
        Debug.Log("Entered");
    }

    void OnTriggerExit()
    {
        this.baseModel.GetComponent<MeshRenderer>().material.color = Color.red;
        onDepress();
        Debug.Log("Exit");
    }
}
