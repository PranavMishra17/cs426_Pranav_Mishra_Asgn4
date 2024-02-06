using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySurge : MonoBehaviour
{
    public string batteryTag = "Battery";
    public int requiredBatteryCount = 3;

    [SerializeField] GameObject gameQuestion;
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
                SpawnAndPlayParticles();
                this.gameQuestion.SetActive(true);
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

    void SpawnAndPlayParticles()
    {
        // Play the one-time effect
        PlayOneTimeEffect();

        audioSource.PlayOneShot(explodeEffect);

        // Start the continuous effect after a delay
        Invoke("PlayContinuousEffect", delayBeforeContinuous);
    }

    void PlayOneTimeEffect()
    {
        // Randomly scatter the spawn position within the specified radius
        Vector3 randomPosition = spawnFire.position + Random.insideUnitSphere * scatterRadius;

        // Spawn and play the one-time particle effect at the random position
        GameObject oneTimeEffectInstance = Instantiate(oneTimeEffect, randomPosition, Quaternion.identity);
        oneTimeEffectInstance.GetComponent<ParticleSystem>().Play();
    }

    void PlayContinuousEffect()
    {
        // Spawn and play the continuous particle effect at the random position
        GameObject continuousEffectInstance = Instantiate(continuousEffect, spawnSmoke.position, Quaternion.identity);
        continuousEffectInstance.GetComponent<ParticleSystem>().Play();
    }
}
