using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Package : NetworkBehaviour {
    private GameObject holder;
    private bool isHeld;
    private float time;
    private Vector3 basePosition;
    
    void Start() {
        this.holder = null;
        this.isHeld = false;
        this.time = 0f;
        this.basePosition = this.transform.position;
    }

    public void setHolder(GameObject holder) {
        if (this.holder)
        {
            return;
        }
        this.holder = holder;
        this.isHeld = true;
    }

    public void drop() {
        this.isHeld = false;
        this.holder = null;
        this.basePosition.y = 0.2f;
    }

    public bool hasHolder()
    {
        return this.holder != null;
    }

    void Update() {
        time += Time.deltaTime;
        if (this.IsServer)
        {
            if (this.isHeld)
            {
                this.basePosition = this.holder.transform.position;
                this.transform.position = this.basePosition;
            }
            else
            {
                this.transform.position = this.basePosition + (Vector3.up * (Mathf.Sin(this.time) * 0.35f));
            }
        }
        Quaternion rotation = this.transform.rotation * Quaternion.Euler(Time.deltaTime * 20, Time.deltaTime * 30, 0);
        this.transform.rotation = rotation;
    }
}