using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour {
    public float speed = 2f;
    public List<Color> colors = new List<Color>();
    private GameObject instantiatedPrefab;
    public GameObject cannon;
    public GameObject bullet;

    [SerializeField] private GameObject spawnedPrefab;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private Camera playerCamera;

    void Update() {        
        if (this.IsOwner) { // makes sure the script is only executed on the owners 
            Vector3 moveDirection = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W)) {
                moveDirection.z = +1f;
            }
            if (Input.GetKey(KeyCode.S)) {
                moveDirection.z = -1f;
            }
            if (Input.GetKey(KeyCode.A)) {
                moveDirection.x = -1f;
            }
            if (Input.GetKey(KeyCode.D)) {
                moveDirection.x = +1f;
            }
            transform.position += moveDirection * speed * Time.deltaTime;

            // If I is pressed spawn the object. If J is pressed destroy the object
            if (Input.GetKeyDown(KeyCode.I) && instantiatedPrefab == null) {
                instantiatedPrefab = Instantiate(spawnedPrefab);
                instantiatedPrefab.GetComponent<NetworkObject>().Spawn(true);
            } else if (Input.GetKeyDown(KeyCode.J) && instantiatedPrefab != null) {
                instantiatedPrefab.GetComponent<NetworkObject>().Despawn(true);
                Destroy(instantiatedPrefab);
                instantiatedPrefab = null;
            }

            if (Input.GetButtonDown("Fire1")) {
                // call the BulletSpawningServerRpc method since client can't spawn objects
                BulletSpawningServerRpc(cannon.transform.position, cannon.transform.rotation);
            }
        }
    }

    // this method is called when the object is spawned
    public override void OnNetworkSpawn() {
        // Set object color
        GetComponent<MeshRenderer>().material.color = colors[(int)OwnerClientId];

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

    // need to add the [ServerRPC] attribute
    [ServerRpc]
    private void BulletSpawningServerRpc(Vector3 position, Quaternion rotation) {  // method name must end with ServerRPC
        // call the BulletSpawningClientRpc method to locally create the bullet on all clients
        BulletSpawningClientRpc(position, rotation);
    }

    [ClientRpc]
    private void BulletSpawningClientRpc(Vector3 position, Quaternion rotation) {
        GameObject newBullet = Instantiate(bullet, position, rotation);
        newBullet.GetComponent<Rigidbody>().velocity += Vector3.up * 2;
        newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.up * 1500);
    }
}