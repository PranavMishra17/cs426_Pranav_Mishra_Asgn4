using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BatterySurge : NetworkBehaviour
{
    public string batteryTag = "Battery";
    public int requiredBatteryCount = 3;


    public GameObject oneTimeEffect; // Particle effect to play once
    public GameObject continuousEffect; // Particle effect to play continuously
    public Transform spawnSmoke;
    public Transform spawnFire;// Spawn point
    public float scatterRadius = 5f; // Radius for scattering effects
    public float delayBeforeContinuous = 1f; // Delay before starting the continuous effect

    public AudioClip explodeEffect;  // Assign your sound effect in the Unity editor
    private AudioSource audioSource;
    private bool poweractive = true;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = explodeEffect;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(batteryTag))
        {
            // Increase the count of batteries in the trigger area
            int batteryCount = CountBatteriesInTrigger();

            Debug.Log("Battery count is " + batteryCount);
            // Check if the required number of batteries is reached
            if (batteryCount >= requiredBatteryCount && poweractive)
            {
                // Call the function to spawn and play particle effects
                SpawnAndPlayParticlesServerRpc();
                poweractive = false;
            }
        }
    }

    int CountBatteriesInTrigger()
    {
        // Find all colliders within the trigger area using Physics.OverlapSphere
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.magnitude / 2f);

        int batteryCount = 0;

        // Count the batteries with the specified tag
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(batteryTag))
            {
                batteryCount++;
            }
        }

        return batteryCount;
    }

    [ServerRpc]
    void SpawnAndPlayParticlesServerRpc()
    {
        // Play the one-time effect
        PlayOneTimeEffectServerRpc();

        audioSource.PlayOneShot(explodeEffect);

        PlayContinuousEffectServerRpc();
    }

    [ServerRpc]
    void PlayOneTimeEffectServerRpc()
    {
        // Randomly scatter the spawn position within the specified radius
        Vector3 randomPosition = spawnFire.position + Random.insideUnitSphere * scatterRadius;

        // Spawn and play the one-time particle effect at the random position
        GameObject oneTimeEffectInstance = Instantiate(oneTimeEffect, randomPosition, Quaternion.identity);
        oneTimeEffectInstance.GetComponent<NetworkObject>().Spawn();
        oneTimeEffectInstance.GetComponent<ParticleSystem>().Play();


    }

    [ServerRpc]
    void PlayContinuousEffectServerRpc()
    {
        // Spawn and play the continuous particle effect at the random position
        GameObject continuousEffectInstance = Instantiate(continuousEffect, spawnSmoke.position, Quaternion.identity);
        continuousEffectInstance.GetComponent<NetworkObject>().Spawn();
        continuousEffectInstance.GetComponent<ParticleSystem>().Play();
    }


}
