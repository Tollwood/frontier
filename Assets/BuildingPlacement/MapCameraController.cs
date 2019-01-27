using UnityEngine;

[RequireComponent(typeof( Camera))]
public class MapCameraController : MonoBehaviour
{

    [Header("Zoom Settings")]
    public float minSize = 30f;
    public float maxSize = 120f;
    public float sensitivity = 10f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void Update()
    {
        HandleZoom();

    }

    private void HandleZoom()
    {
        float size = cam.orthographicSize;
        size += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        size = Mathf.Clamp(size, minSize, maxSize);
        cam.orthographicSize = size;
    }
}
