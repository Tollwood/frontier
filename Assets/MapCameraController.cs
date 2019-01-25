
using UnityEngine;

[RequireComponent(typeof( Camera))]
public class MapCameraController : MonoBehaviour
{
    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void Update()
    {
        float fov =cam.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        cam.fieldOfView = fov;
    }
}
