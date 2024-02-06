using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meow : MonoBehaviour
{
    // Start is called before the first frame updat
    public AudioClip meoweffect; // Assign your sound effect in the Unity editor
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = meoweffect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
