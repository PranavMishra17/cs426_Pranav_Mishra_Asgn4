using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour {
    public float speed = 2f;
    public float rotationSpeed = 70f;
    public List<Color> colors = new List<Color>();
    public GameObject hand;

    [SerializeField] private AudioListener audioListener;
    [SerializeField] private Camera playerCamera;
    private Package dataPackage;

    void Start() {
        this.dataPackage = null;
    }

    void Update() {        
        if (this.IsOwner) { // makes sure the script is only executed on the owners
            Vector3 moveDirection = new Vector3(0, 0, 0);
            Quaternion rotation = Quaternion.identity;
            if (Input.GetKey(KeyCode.W)) {
                moveDirection = this.transform.forward;
            }
            if (Input.GetKey(KeyCode.S)) {
                moveDirection = this.transform.forward * -1f;
            }
            if (Input.GetKey(KeyCode.A)) {
                rotation = Quaternion.Euler(0f, -1f * rotationSpeed * Time.deltaTime, 0f);
            }
            if (Input.GetKey(KeyCode.D)) {
                rotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f);
            }
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.rotation *= rotation;

            if (Input.GetKeyDown(KeyCode.E))
            {
                AttemptInteractServerRpc();
            }
        }
    }

    [ServerRpc]
    private void AttemptInteractServerRpc() {
        if (this.dataPackage) {
            this.dropPackage();
        } else {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 2f, LayerMask.GetMask("Interactables")))
            {
                this.dataPackage = hit.transform.gameObject.GetComponent<Package>();
                if (this.dataPackage.hasHolder())
                {
                    return;
                }
                Debug.Log("Picked up package.");
                this.dataPackage.setHolder(this.hand);
            } else
            {
                Debug.Log("Miss");
            }
        }
    }


    public void dropPackage() {
        Debug.Log("Dropping package.");
        this.dataPackage.drop();
        this.dataPackage = null;
    }

    // this method is called when the object is spawned
    public override void OnNetworkSpawn() {
        Debug.Log("Spawned player.");
        // Set object color
        GetComponent<MeshRenderer>().material.color = colors[(int)this.OwnerClientId % colors.Count];

        // If the player is the owner, enable audioListener and playerCamera
        if (this.IsOwner) {
            Destroy(GameObject.Find("Host"));
            Destroy(GameObject.Find("Client"));
            Destroy(GameObject.Find("EnterCode"));
            if (!this.IsHost) {
                Destroy(GameObject.Find("JoinCode"));
            }
            audioListener.gameObject.SetActive(true);
            playerCamera.gameObject.SetActive(true);
            audioListener.enabled = true;
            playerCamera.enabled = true;
        }
    }
}