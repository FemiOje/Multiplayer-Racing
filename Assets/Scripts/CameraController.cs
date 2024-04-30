using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    Camera _camera;
    [SerializeField] Transform[] cameraPositions;
    Vector2 _rightStickInputValue;
    int _cameraIndex = 0;
    float _rotationSpeed = 200f;

    InputAction _cameraRotateAction;

    public Transform player;
    Rigidbody _playerRb;
    public Vector3 offset;
    [SerializeField] float _lerpSpeed;

    private void OnEnable()
    {
        _cameraRotateAction = GetComponent<PlayerInput>().actions.FindAction("RotateCamera");
    }

    void Start()
    {
        _playerRb = player.GetComponent<Rigidbody>();
        _camera = GetComponent<Camera>();
        _camera.transform.position = cameraPositions[_cameraIndex].gameObject.transform.position;
    }

    private void FixedUpdate()
    {
        // RotateCamera();

        Vector3 playerForward = (_playerRb.velocity + player.transform.forward).normalized;
        transform.position = Vector3.Lerp(
            transform.position,
            player.position + player.transform.TransformVector(offset) + playerForward * (-5f),
            _lerpSpeed * Time.deltaTime
        );
        transform.LookAt(player);
    }

    public void ToggleCameras(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _cameraIndex = (_cameraIndex + 1) % cameraPositions.Length;
            _camera.transform.position = cameraPositions[_cameraIndex].gameObject.transform.position;
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
                float rotationX = _rightStickInputValue.y * _rotationSpeed * Time.deltaTime;
                float rotationY = _rightStickInputValue.x * _rotationSpeed * Time.deltaTime;

                // Calculate current angles of the camera
                Vector3 currentAngles = _camera
                    .gameObject
                    .transform
                    .eulerAngles;

                // Apply rotation limits
                float newXAngle = Mathf.Clamp(currentAngles.x - rotationX, 5f, 80f); // Adjust the min and max values as needed
                float newYAngle = currentAngles.y + rotationY;

                // Apply rotation to camera
                _camera.gameObject.transform.rotation = Quaternion.Euler(
                    newXAngle,
                    newYAngle,
                    0f
                );
            }
        }
    }

    private void UpdateCameraPosition() { }
}
