using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PushSphere : MonoBehaviour
{
    public Rigidbody sphereRigidbody;
    public Camera playerCamera; // Assuming this is the main camera
    public float pushForce = 10f;
    public float activationDistance = 3f;
    public GameObject collisionEffect;
    public GameObject spotLight;
    public GameObject package;

    private Transform playerTransform;

    void Start()
    {

    }

    void Update()
    {
        // Check if the playerTransform reference is valid
        if (playerTransform == null)
        {
            // Find the player object dynamically by tag
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            // If player object is found, get its transform
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;

                // Find the camera among the children of the player object
                foreach (Transform child in playerTransform)
                {
                    Camera childCamera = child.GetComponent<Camera>();
                    if (childCamera != null)
                    {
                        playerCamera = childCamera;
                        break;
                    }
                }

                if (playerCamera == null)
                {
                    Debug.LogError("Camera component not found among the children of the player object!");
                }
            }
        }

        // Calculate the distance between the player and the sphere
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Check if the "P" key is pressed and the player is near the sphere
        if (Input.GetKey(KeyCode.P) && distanceToPlayer <= activationDistance)
        {
            // Determine the direction to push the sphere
            Vector3 pushDirection = (playerCamera.transform.position - transform.position).normalized;

            // Apply force to the sphere in the direction of the camera/player
            sphereRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Fan")
        {
            PlayOneTimeEffect(collision.transform);
        }   
    }
    void PlayOneTimeEffect(Transform spawnFire)
    {
        spotLight.SetActive(true);
        //package.SetActive(true);

        // Spawn and play the one-time particle effect at the random position
        GameObject oneTimeEffectInstance = Instantiate(collisionEffect, spawnFire.position, Quaternion.identity);
        oneTimeEffectInstance.GetComponent<ParticleSystem>().Play();

        Destroy(oneTimeEffectInstance, 4f);

    }

}
