using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class PlatformObjectCounter : NetworkBehaviour
{
    public string objectTag = "Player";
    private int objectsOnPlatform = 0;
    public Transform platform;
    public Transform targetPosition;
    public float moveSpeed;
    public int plCount;

    public AudioClip countEffect;
    public AudioClip completeEffect;// Assign your sound effect in the Unity editor
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = countEffect;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(objectTag))
        {
            
            objectsOnPlatform++;
            if (objectsOnPlatform != plCount && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(countEffect);
            }
            Debug.Log("Object entered platform. Count: " + objectsOnPlatform);

        }
    }

    private void Update()
    {
        if (objectsOnPlatform == plCount)
        {
            platform.position = Vector3.MoveTowards(platform.position, targetPosition.position, moveSpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(completeEffect);
                audioSource = null;
            }
            
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
