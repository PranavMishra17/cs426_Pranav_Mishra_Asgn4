using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TPC : NetworkBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    public GameObject bloodSplatterPrefab;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [SerializeField] private GameObject hand;
    [SerializeField] private Camera playerCamera;

    [HideInInspector]
    public bool canMove = true;
    private Package dataPackage;

    public AudioClip deathEffect;
    public AudioClip laughEffect; // Assign your sound effect in the Unity editor
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = deathEffect;
    }

    void Update()
    {
        if (this.IsOwner)
        {
            if (characterController.isGrounded)
            {
                // We are grounded, so recalculate move direction based on axes
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);
                float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
                float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
                moveDirection = (forward * curSpeedX) + (right * curSpeedY);

                if (Input.GetButton("Jump") && canMove)
                {
                    moveDirection.y = jumpSpeed;
                }
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y -= gravity * Time.deltaTime;

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
            if (canMove)
            {
                rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
                rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
                this.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
                transform.eulerAngles = new Vector2(0, rotation.y);
            }

            // Object Interaction
            if (Input.GetKeyDown(KeyCode.E))
            {
                AttemptInteractServerRpc();
                if (!this.IsServer)
                {
                    this.AttemptInteract();
                }
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.tag == "Fan")
        {
            SpawnBloodSplatter();
            audioSource.PlayOneShot(deathEffect);
        }
    }

    private void AttemptInteract()
    {
        if (this.dataPackage)
        {
            this.dropPackage();
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 2f, LayerMask.GetMask("Interactables")))
            {
                this.dataPackage = hit.transform.gameObject.GetComponent<Package>();
                audioSource.PlayOneShot(laughEffect);
                if (this.dataPackage.hasHolder())
                {
                    this.dataPackage = null;
                    return;
                }
                this.dataPackage.setHolder(this.hand);
            }
        }
    }

    [ServerRpc]
    private void AttemptInteractServerRpc()
    {
        this.AttemptInteract();
    }

    public void dropPackage()
    {
        this.dataPackage.drop();
        this.dataPackage = null;
    }

    void SpawnBloodSplatter()
    {
        Debug.Log("Blood Splatter called");
        // Spawn the BloodSplatter particle effect at the trigger's position and rotation
        GameObject bloodSplatter = Instantiate(bloodSplatterPrefab, transform.position, Quaternion.identity);
        // bloodSplatter.GetComponent<NetworkObject>().Spawn();
        //bloodSplatter.GetComponent<NetworkObject>().GetComponent<ParticleSystem>().Play();

        // Play the particle effect
        var particleSystem = bloodSplatter.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
            Debug.Log("inside play called"+ particleSystem);
        }
        Invoke("DisableGameObject", 2f);
        // Optionally, destroy the particle effect after its duration
        //Destroy(bloodSplatter, particleSystem.main.duration);
    }
    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        // If the player is the owner, enable audioListener and playerCamera
        if (this.IsOwner)
        {
            characterController = GetComponent<CharacterController>();
            rotation.y = transform.eulerAngles.y;
            this.dataPackage = null;
            this.transform.position = GameState.GetSpawnLoc();

            // Clean UI
            Destroy(GameObject.Find("Host"));
            Destroy(GameObject.Find("Client"));
            Destroy(GameObject.Find("EnterCode"));
            if (!this.IsHost)
            {
                Destroy(GameObject.Find("Code"));
            }
            playerCamera.gameObject.SetActive(true);
            playerCamera.enabled = true;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (this.IsServer)
        {
            if (this.dataPackage != null)
            {
                this.dropPackage();
            }
        }
    }
}
