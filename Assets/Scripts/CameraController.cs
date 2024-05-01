using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    Camera _camera;

    [SerializeField]
    Transform[] cameraPositions;
    int _cameraIndex = 0;
    InputAction _cameraRotateAction;
    Vector2 _rightStickInputValue;

    public Transform player;
    Rigidbody _playerRb;
    public Vector3 offset;

    [SerializeField]
    float _lerpSpeed;

    [SerializeField]
    float _cameraRotateSpeed;

    private void OnEnable()
    {
        _cameraRotateAction = player.gameObject
            .GetComponent<PlayerInput>()
            .actions.FindAction("RotateCamera");
    }

    void Start()
    {
        _playerRb = player.GetComponent<Rigidbody>();
        _camera = GetComponent<Camera>();
        _camera.transform.position = cameraPositions[_cameraIndex].gameObject.transform.position;
    }

    private void FixedUpdate()
    {
        UpdateCameraRotation();
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 playerForward = (_playerRb.velocity + player.transform.forward).normalized;
        transform.position = Vector3.Lerp(
            transform.position,
            cameraPositions[_cameraIndex].gameObject.transform.position + playerForward * (-5f),
            _lerpSpeed * Time.deltaTime
        );
        transform.LookAt(player);
    }

    private void UpdateCameraRotation()
    {
        _rightStickInputValue = _cameraRotateAction.ReadValue<Vector2>();

        if (_rightStickInputValue.magnitude != 0)
        {
            transform.RotateAround(
                player.transform.position,
                Vector3.up,
                _rightStickInputValue.x * _cameraRotateSpeed * Time.deltaTime
            );
            transform.RotateAround(
                player.transform.position,
                Vector3.right,
                _rightStickInputValue.y * _cameraRotateSpeed * Time.deltaTime
            );
        }
    }

    public void ToggleCameras(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IncreaseCameraIndex();
        }
    }

    private int IncreaseCameraIndex()
    {
        return _cameraIndex = (_cameraIndex + 1) % cameraPositions.Length;
    }
}
