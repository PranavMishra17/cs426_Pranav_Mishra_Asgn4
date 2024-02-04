using UnityEngine;

public class ButtonCollider : MonoBehaviour
{
    private ButtonLogic button;

    void Start()
    {
        this.button = this.transform.parent.gameObject.GetComponent<ButtonLogic>();
    }

    void OnTriggerEnter(Collider other)
    {
        this.button.OnEnter(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        this.button.OnExit(other.gameObject);
    }
}
