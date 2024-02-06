using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class fanRotation : MonoBehaviour
{
    public GameObject gameQuestion;
    public float rotationSpeed = 5f;
    public float lerpSpeed = 2f;
    private int collisionCount = 0;
    private AudioSource ass;

    public GameObject smokeEffect; // Particle effect to play continuously
    public Transform spawnSmoke;

    public int ratsKilled = 2;

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
        if (collisionCount >= ratsKilled)
        {
            ass = gameObject.GetComponent<AudioSource>();
            ass.Stop();
            PlayContinuousEffect();
            gameQuestion.SetActive(true);
            //enabled = false; // Disable this script
        }
    }

    void PlayContinuousEffect()
    {
        // Spawn and play the continuous particle effect at the random position
        GameObject continuousEffectInstance = Instantiate(smokeEffect, spawnSmoke.position, Quaternion.identity);
        continuousEffectInstance.GetComponent<ParticleSystem>().Play();
    }
}
