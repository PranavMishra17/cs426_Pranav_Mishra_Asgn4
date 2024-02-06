using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableListner : MonoBehaviour
{
    public AudioListener Listener;
    // Start is called before the first frame update
    void Start()
    {
        Listener = gameObject.GetComponent<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        // Find a GameObject with the "Player" tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Check if a player GameObject is found
        if (playerObject != null)
        {
            // Remove the AudioListener component if it exists

            if (Listener != null)
            {
                Destroy(Listener);
            }
        }
    }
}
