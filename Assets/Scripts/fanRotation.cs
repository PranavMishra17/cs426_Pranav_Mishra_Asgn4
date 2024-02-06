using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanRotation : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float lerpSpeed = 2f;
    private int collisionCount = 0;
    private AudioSource ass;

    void Update()
    {
        // Rotate the object
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // Increase collision count
            collisionCount++;

            // Slow down the rotation using Lerp
            StartCoroutine(SlowDownRotation());
        }
    }

    IEnumerator SlowDownRotation()
    {
        float targetSpeed = rotationSpeed * 0.5f;

        while (rotationSpeed > targetSpeed)
        {
            // Lerp to gradually decrease rotation speed
            rotationSpeed = Mathf.Lerp(rotationSpeed, targetSpeed, Time.deltaTime * lerpSpeed);

            yield return null;
        }

        // Ensure the rotation speed is exactly half to avoid small rotations over time
        rotationSpeed = targetSpeed;

        // If there were two collisions, stop further rotation
        if (collisionCount >= 3)
        {
            ass = gameObject.GetComponent<AudioSource>();
            ass.Stop();
            //enabled = false; // Disable this script
        }
    }
}
