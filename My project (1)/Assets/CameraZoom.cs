using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera Cam;
    public float TargetZoom = 3f;
    private float ScrollData;
    public float ZoomSpeed = 3f;

    void Start()
    {
        Cam = GetComponent<Camera>();
        TargetZoom = Cam.orthographicSize;
    }

    void Update()
    {
        ScrollData = Input.GetAxis("Mouse ScrollWheel");
        TargetZoom = TargetZoom - ScrollData;

        // Limit how far the camera can zoom in or out.
        TargetZoom = Mathf.Clamp(TargetZoom, 3f, 6f);

        // Smoothly move toward the target orthographic size.
        Cam.orthographicSize = Mathf.Lerp(
            Cam.orthographicSize,
            TargetZoom,
            Time.deltaTime * ZoomSpeed
        );
    }
}