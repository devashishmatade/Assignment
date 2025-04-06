using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // The player
    public Vector3 offset = new Vector3(0, 2, -6);
    public float followSpeed = 10f;

    [Header("Rotation Settings")]
    public float mouseSensitivity = 3f;
    public float verticalMin = -60f;
    public float verticalMax = 80f;

    private float yaw;
    private float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleCameraRotation();
        HandleCameraPosition();
    }

    void HandleCameraRotation()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, verticalMin, verticalMax);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.rotation = rotation;
    }

    void HandleCameraPosition()
    {
        // Desired camera position based on rotation and offset
        Vector3 desiredPosition = target.position + transform.rotation * offset;

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}