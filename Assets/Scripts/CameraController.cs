using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    private CinemachineTransposer cinemachineTransposer;

    private Vector3 targetFollowOffset;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {

        HandleMovement();
        HandleRotation();
        HandleZoom();

    }

    private void HandleMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();


        float moveSpeed = 5f;

        Vector3 moveVector = transform.forward * inputMoveDir.y   + transform.right * inputMoveDir.x;

        transform.position += moveSpeed * Time.deltaTime * moveVector;

    }

    private void HandleRotation()
    {

        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();
        
        float rotateSpeed = 100f;
        transform.eulerAngles += rotateSpeed * Time.deltaTime * rotationVector;

    }

    private void HandleZoom()
    {
        float zoomIncreaseAmount = 1f;
        
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() *zoomIncreaseAmount;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}
