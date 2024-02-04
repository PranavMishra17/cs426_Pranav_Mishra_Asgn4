using UnityEngine;

public class ButtonCollider : MonoBehaviour
{
    private ButtonLogic button;

    void Start()
    {
        this.button = this.transform.parent.gameObject.GetComponent<ButtonLogic>();
        if (this.button == null)
        {
            Debug.Log("Couldn't find parent button logic.");
        }
    }

    void OnTriggerEnter()
    {
        this.button.OnEnter();
    }

    void OnTriggerExit()
    {
        this.button.OnExit();
    }
}
