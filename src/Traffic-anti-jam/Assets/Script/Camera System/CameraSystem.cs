using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSystem : MonoBehaviour
{
    private Vector3 direction;
    private Vector2 directInputValue;
    private float rotationValue;
    private float targetFoV = 60f;
    private float zoomStep = 5f;
    private float zoomSpeed = 10f;
    private float minFoV = 20f;
    private float maxFoV = 80f;
    private float moveSpeed = 35f;
    private float axisX = 20f;
    private float axisZ = 20f;
    private float rotateSoeed = 30f;

    [SerializeField]
    private CinemachineVirtualCamera vCamera;
    private CinemachineTransposer transposer;

    private void Start()
    {
        transposer = vCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void FixedUpdate()
    {
        CameraMovement();
        CameraRotation();
        CameraZoom();
    }

    public void CameraMovementDirection(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            directInputValue = context.ReadValue<Vector2>();
            direction = new Vector3(directInputValue.x, 0.0f, directInputValue.y);
        }
        else if (context.canceled)
        {
            direction = Vector3.zero;
        }
    }

    private void CameraMovement()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -axisX, axisX),
            0,
            Mathf.Clamp(transform.position.z, -axisZ + transposer.m_FollowOffset.z / 2, axisZ)
        );

        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.Self);
    }

    public void CameraRotateDirection(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rotationValue = context.ReadValue<float>();
        }
        else if (context.canceled)
        {
            rotationValue = 0;
        }
    }

    private void CameraRotation()
    {
        transform.Rotate(0.0f, rotationValue * rotateSoeed * Time.deltaTime, 0.0f, Space.World);
    }

    public void CameraZoomInputValue(InputAction.CallbackContext contest)
    {
        if (contest.performed)
        {
            targetFoV += contest.ReadValue<float>() * zoomStep * Time.deltaTime;
        }
        else if (contest.canceled)
        {
            targetFoV += 0.0f;
        }

        targetFoV = Mathf.Clamp(targetFoV, minFoV, maxFoV);
    }

    private void CameraZoom()
    {
        vCamera.m_Lens.FieldOfView = Mathf.Lerp(
            vCamera.m_Lens.FieldOfView,
            targetFoV,
            Time.deltaTime * zoomSpeed
        );
    }
}
