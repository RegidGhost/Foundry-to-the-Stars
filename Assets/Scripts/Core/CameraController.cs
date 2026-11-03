using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 1f;
    [SerializeField] private float panSmoothness = 5f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 15f;

    private Camera _camera;
    private Vector3 _targetPosition;
    private float _targetZoom;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _targetPosition = transform.position;
        _targetZoom = _camera.orthographicSize;
    }

    private void Update()
    {
        HandlePan();
        HandleZoom();
        ApplySmoothness();
    }

    private void HandlePan()
    {
        // Панорамирование средней кнопкой мыши
        if (Input.GetMouseButton(2))
        {
            float deltaX = -Input.GetAxis("Mouse X") * panSpeed;
            float deltaY = -Input.GetAxis("Mouse Y") * panSpeed;

            // Учитываем зум: чем дальше камера, тем быстрее движение
            float zoomFactor = _camera.orthographicSize / 5f;
            _targetPosition += new Vector3(deltaX, deltaY, 0) * zoomFactor;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            _targetZoom -= scroll * zoomSpeed;
            _targetZoom = Mathf.Clamp(_targetZoom, minZoom, maxZoom);
        }
    }

    private void ApplySmoothness()
    {
        // Плавное движение
        transform.position = Vector3.Lerp(
            transform.position,
            _targetPosition,
            Time.deltaTime * panSmoothness
        );

        _camera.orthographicSize = Mathf.Lerp(
            _camera.orthographicSize,
            _targetZoom,
            Time.deltaTime * panSmoothness
        );
    }
}