using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformObjectCounter : MonoBehaviour
{
    public string objectTag = "Player";
    private int objectsOnPlatform = 0;
    public Transform platform;
    public Transform targetPosition;
    public float moveSpeed;
    public int plCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(objectTag))
        {
            objectsOnPlatform++;
            Debug.Log("Object entered platform. Count: " + objectsOnPlatform);

        }
    }

    private void Update()
    {
        if (objectsOnPlatform == plCount)
        {
            platform.position = Vector3.MoveTowards(platform.position, targetPosition.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(objectTag))
        {
            objectsOnPlatform--;
            Debug.Log("Object exited platform. Count: " + objectsOnPlatform);
        }
    }
}
