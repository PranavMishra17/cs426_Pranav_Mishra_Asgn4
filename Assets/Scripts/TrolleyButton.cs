using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TrolleyButton : NetworkBehaviour
{
    public Transform platform; // The platform that moves between point A and B
    public Transform pointA;   // Point A position
    public Transform pointB;   // Point B position
    public float moveSpeed = 5f; // Speed of the trolley
    public float activationDistance = 7f; // Distance at which the button can be pressed

    // private bool movingToB = true; // Flag to indicate the direction of movement
    private NetworkVariable<bool> movingToB = new NetworkVariable<bool>(false);

    public override void OnNetworkSpawn()
    {
        if (this.IsOwner)
        {
            this.platform.position = pointA.position;
        }
    }

    void Update()
    {
        // Check if the player is near the button
        if (this.IsPlayerNear())
        {
            // Check if the button is pressed (You can replace "Fire1" with your input)
            if (Input.GetButtonDown("Fire1"))
            {
                this.ChangeDirectionServerRpc();
            }
        }
        this.MovePlatform();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeDirectionServerRpc()
    {
        this.movingToB.Value = !this.movingToB.Value;
    }

    bool IsPlayerNear()
    {
        if (this.IsClient)
        {
            GameObject player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().gameObject;

            if (player != null)
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                return distance <= activationDistance;
            }
            return false;
        }
        return false;
    }

    void MovePlatform()
    {
        // Calculate the target position based on the direction
        Transform targetPosition;
        if (this.movingToB.Value)
        {
            Debug.Log("Moving toward B");
            targetPosition = pointB;
        }
        else
        {
            Debug.Log("Moving toward A");
            targetPosition = pointA;
        }

        // Move the platform towards the target position
        platform.position = Vector3.MoveTowards(platform.position, targetPosition.position, moveSpeed * Time.deltaTime);
    }

}
