using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObjectCounter : MonoBehaviour
{
    public string objectTag = "Player";
    private int objectsOnPlatform = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(objectTag))
        {
            objectsOnPlatform++;
            Debug.Log("Object entered platform. Count: " + objectsOnPlatform);
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
