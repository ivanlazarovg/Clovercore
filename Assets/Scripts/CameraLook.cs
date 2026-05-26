using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseXSensitivity = 100f;

    public float mouseYSensitivity = 50f;

    public Transform playerBody;

    public float xRotation = 0f;

    public Camera cam;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float cameraSensitivityResolution = cam.fieldOfView / 60;
        float mouseX = Input.GetAxis("Mouse X") * (mouseXSensitivity * cameraSensitivityResolution) * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * (mouseYSensitivity * cameraSensitivityResolution) * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

    }
}
