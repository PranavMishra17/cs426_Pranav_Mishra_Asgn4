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
    public float activationDistance = 3f; // Distance at which the button can be pressed

    // private bool movingToB = true; // Flag to indicate the direction of movement
    private NetworkVariable<bool> movingToB = new NetworkVariable<bool>(false);

    void OnNetworkSpawn()
    {
        if (this.IsServer)
        {
            this.platform = pointA;
            this.movingToB.Value = false;
        }
    }

    void Update()
    {
        // Check if the player is near the button
        if (IsPlayerNear())
        {
            // Check if the button is pressed (You can replace "Fire1" with your input)
            if (Input.GetButtonDown("Fire1"))
            {
                // Toggle the direction of movement when the button is pressed
                movingToB.Value = !movingToB.Value;
            }
        }

        // Move the platform based on the direction
        MovePlatform();
    }

    bool IsPlayerNear()
    {
        if (this.IsClient)
        {
            GameObject player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().gameObject;
            // GameObject player = GameObject.FindGameObjectWithTag("Player");

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
        Transform targetPosition = movingToB.Value ? pointB : pointA;

        // Move the platform towards the target position
        platform.position = Vector3.MoveTowards(platform.position, targetPosition.position, moveSpeed * Time.deltaTime);
    }

}
