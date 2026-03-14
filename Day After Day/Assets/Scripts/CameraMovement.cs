using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 dragOrigin;

    public float zoomStep, minZoomSize, maxZoomSize;

    public SpriteRenderer mapRenderer;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    void Awake()
    {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    }

    void Update()
    {
        PanCamera();
        HandleZoom();
    }

    void PanCamera()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            dragOrigin = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }
    }

    void HandleZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;
    
        if (scroll == 0) return;
    
        Vector3 mouseBeforeZoom = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    
        float newSize = cam.orthographicSize - scroll * zoomStep * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(newSize, minZoomSize, maxZoomSize);
    
        Vector3 mouseAfterZoom = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        cam.transform.position += mouseBeforeZoom - mouseAfterZoom;
    
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    public Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}