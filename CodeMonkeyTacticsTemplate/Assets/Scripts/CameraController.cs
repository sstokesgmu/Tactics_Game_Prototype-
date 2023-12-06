using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;

    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    private const float MIN_FOLLOW_Z_OFFSET = -15f;
    private const float MAX_FOLLOW_Z_OFFSET = -5f;

    public CinemachineTransposer cmTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        //Camera Zoom Components
        cmTransposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cmTransposer.m_FollowOffset;
    }


    // Update is called once per frame
    void Update()
    {
        //Camera Move
        Vector3 inputMoveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
            inputMoveDir.z = +1f;
        if (Input.GetKey(KeyCode.S))
            inputMoveDir.z = -1f;
        if (Input.GetKey(KeyCode.A))
            inputMoveDir.x = -1f;
        if (Input.GetKey(KeyCode.D))
            inputMoveDir.x = +1f;

        float moveSpeed= 5f;
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;

        //Camera Rotate
        Vector3 rotationVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q))
            rotationVector.y = +1f;
        if (Input.GetKey(KeyCode.E))
            rotationVector.y = -1f;

        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;

        float zoomAmount = 1f;
        
        if(Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomAmount;
            targetFollowOffset.z += zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomAmount;
            targetFollowOffset.z -= zoomAmount;
        }
        float zoomSpeed = 5f;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        targetFollowOffset.z = Mathf.Clamp(targetFollowOffset.z, MIN_FOLLOW_Z_OFFSET, MAX_FOLLOW_Z_OFFSET);
        cmTransposer.m_FollowOffset = Vector3.Lerp(cmTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}
