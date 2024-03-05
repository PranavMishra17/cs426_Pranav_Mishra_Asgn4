using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSphere : MonoBehaviour
{
    public GameObject ball;
    public Transform spawnposition;
    public GameObject SPOTLIGHT;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Spawnball();
        }
    }

    public void Spawnball()
    {
        GameObject ballspawned = Instantiate(ball, spawnposition.position, Quaternion.identity);
        SPOTLIGHT.SetActive(false);
    }
}
