using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Pan")]
    [SerializeField] private float panSmoothness = 12f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 15f;

    [Header("Bounds")]
    [SerializeField] private bool clampToBounds = true;
    [SerializeField] private int gridWidth = 64;
    [SerializeField] private int gridHeight = 64;

    private Camera _camera;
    private Vector3 _targetPosition;
    private float _targetZoom;
    private Vector3 _lastMousePos;
    private bool _isPanning;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        Vector3 p = transform.position;
        if (p.z >= 0f) p.z = -10f;
        transform.position = p;
        _targetPosition = p;
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
        if (Input.GetMouseButtonDown(2)) { _isPanning = true; _lastMousePos = Input.mousePosition; }
        if (Input.GetMouseButtonUp(2)) _isPanning = false;
        if (!_isPanning) return;

        Vector3 delta = Input.mousePosition - _lastMousePos;
        _lastMousePos = Input.mousePosition;

        float worldPerPixel = (_camera.orthographicSize * 2f) / Screen.height;
        _targetPosition -= new Vector3(delta.x, delta.y, 0f) * worldPerPixel;
        ClampPosition();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) <= 0.001f) return;
        _targetZoom -= scroll * zoomSpeed;
        _targetZoom = Mathf.Clamp(_targetZoom, minZoom, maxZoom);
        ClampPosition();
    }

    private void ApplySmoothness()
    {
        float t = 1f - Mathf.Exp(-panSmoothness * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, _targetPosition, t);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetZoom, t);
    }

    private void ClampPosition()
    {
        if (!clampToBounds) { _targetPosition.z = -10f; return; }

        float halfH = _camera.orthographicSize;
        float halfW = halfH * _camera.aspect;

        float minX = halfW, maxX = gridWidth - halfW;
        float minY = halfH, maxY = gridHeight - halfH;
        if (minX > maxX) { minX = maxX = gridWidth * 0.5f; }
        if (minY > maxY) { minY = maxY = gridHeight * 0.5f; }

        _targetPosition.x = Mathf.Clamp(_targetPosition.x, minX, maxX);
        _targetPosition.y = Mathf.Clamp(_targetPosition.y, minY, maxY);
        _targetPosition.z = -10f;
    }
}