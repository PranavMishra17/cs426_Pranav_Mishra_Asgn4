using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public float activationDistance = 10f;

    void Update()
    {
        // Check if any camera is near the billboard
        Camera nearCamera = GetNearbyCamera();

        if (nearCamera != null)
        {
            // Ensure the billboard faces the nearby camera
            transform.LookAt(nearCamera.transform);
        }
    }

    Camera GetNearbyCamera()
    {
        // Find all cameras in the scene
        Camera[] allCameras = Camera.allCameras;

        foreach (Camera camera in allCameras)
        {
            // Check the distance between the billboard and the camera
            float distance = Vector3.Distance(transform.position, camera.transform.position);

            // If the camera is within the activation distance, return it
            if (distance < activationDistance)
            {
                return camera;
            }
        }

        // No nearby camera found
        return null;
    }
}
