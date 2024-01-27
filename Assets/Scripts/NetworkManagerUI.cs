using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour {
    [SerializeField] private Button host_btn;
    [SerializeField] private Button client_btn;

    void Awake() {
        // add a listener to the host & client buttons
        host_btn.onClick.AddListener(() => { NetworkManager.Singleton.StartHost(); });
        client_btn.onClick.AddListener(() => { NetworkManager.Singleton.StartClient(); });
    }
}