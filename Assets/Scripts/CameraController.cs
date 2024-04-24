using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Camera[] cameras;

    [SerializeField]
    GameObject car;
    Vector3 carPosition;
    Vector2 _rightStickInputValue;

    private int currentCameraIndex = 0;
    private float rotationSpeed = 200f;

    private InputAction _cameraRotateAction;

    private void OnEnable()
    {
        // Assign the _cameraRotateAction in the OnEnable method
        _cameraRotateAction = GetComponent<PlayerInput>().actions.FindAction("RotateCamera");
    }

    void Start()
    {
        // Ensure only the first camera is active at start
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == 0);
        }
    }

    private void LateUpdate()
    {
        carPosition = car.transform.position;
        RotateCamera();
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

    private void RotateCamera()
    {
        if (_cameraRotateAction != null)
        {
            _rightStickInputValue = _cameraRotateAction.ReadValue<Vector2>();

            // Apply dead zone to input values
            if (_rightStickInputValue.magnitude < 0.1f)
            {
                _rightStickInputValue = Vector2.zero;
            }

            if (_rightStickInputValue != Vector2.zero)
            {
                // Rotate the camera around the car based on input
                float rotationX = _rightStickInputValue.y * rotationSpeed * Time.deltaTime;
                float rotationY = _rightStickInputValue.x * rotationSpeed * Time.deltaTime;

                cameras[currentCameraIndex].gameObject.transform.RotateAround(carPosition, Vector3.up, rotationY);
                cameras[currentCameraIndex].gameObject.transform.RotateAround(carPosition, Vector3.right, -rotationX);
            }
        }
    }
}
