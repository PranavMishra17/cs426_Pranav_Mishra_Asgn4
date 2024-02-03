using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyButton : MonoBehaviour
{

    public Transform platform; // The platform that moves between point A and B
    public Transform pointA;   // Point A position
    public Transform pointB;   // Point B position
    public float moveSpeed = 5f; // Speed of the trolley
    public float activationDistance = 3f; // Distance at which the button can be pressed

    private bool movingToB = true; // Flag to indicate the direction of movement

    void Update()
    {
        // Check if the player is near the button
        if (IsPlayerNear())
        {
            // Check if the button is pressed (You can replace "Fire1" with your input)
            if (Input.GetButtonDown("Fire1"))
            {
                // Toggle the direction of movement when the button is pressed
                movingToB = !movingToB;
            }
        }

        // Move the platform based on the direction
        MovePlatform();
    }

    bool IsPlayerNear()
    {
        // Assuming the player has a "Player" tag, you can customize this based on your setup
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            return distance <= activationDistance;
        }

        return false;
    }

    void MovePlatform()
    {
        // Calculate the target position based on the direction
        Transform targetPosition = movingToB ? pointB : pointA;

        // Move the platform towards the target position
        platform.position = Vector3.MoveTowards(platform.position, targetPosition.position, moveSpeed * Time.deltaTime);
    }

}
