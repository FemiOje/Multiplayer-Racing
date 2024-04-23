using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Camera[] cameras; // Array to hold the cameras
    private int currentCameraIndex = 0; // Index of the current active camera

    void Start()
    {
        // Ensure only the first camera is active at start
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == 0);
        }
    }

    public void ToggleCameras(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Disable the current camera
            cameras[currentCameraIndex].gameObject.SetActive(false);

            // Increment the camera index, wrapping around if necessary
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

            // Enable the next camera
            cameras[currentCameraIndex].gameObject.SetActive(true);
        }
    }
}
