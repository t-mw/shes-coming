using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLogic : MonoBehaviour
{

    public CinemachineBrain cameraBrain;
    public List<CinemachineVirtualCamera> virtualCameras;
    public CinemachineVirtualCamera startCamera;

    private CinemachineVirtualCamera currentCamera;


    private void Start()
    {
        currentCamera = startCamera;
    }

    public void GoNextObject()
    {
        if (virtualCameras.Count > 1)
        {
            virtualCameras.Remove(currentCamera);
            cameraBrain.ActiveVirtualCamera.Priority = 0;

            int nextCameraIndex = Random.Range(0, virtualCameras.Count);
            virtualCameras[nextCameraIndex].Priority = 10;

            currentCamera = virtualCameras[nextCameraIndex];
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            GoNextObject();
        }
    }



}
