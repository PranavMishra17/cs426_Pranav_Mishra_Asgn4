using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 2f;

    void Update()
    {
        if (!IsOwner) return;
        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) {
            moveDirection.z = +1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveDirection.z = -1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveDirection.x = -1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveDirection.x = +1f;
        }
        transform.position += Vector3.Normalize(moveDirection) * speed * Time.deltaTime;
    }
}