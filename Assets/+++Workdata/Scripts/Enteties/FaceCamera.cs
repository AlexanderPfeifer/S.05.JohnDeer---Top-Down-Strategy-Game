using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera[] targetCameraArray;
    private readonly List<Camera> targetCameraList = new();
    private Transform currentCamFollow;

    private void Start()
    {
        targetCameraArray = FindObjectsByType<Camera>(FindObjectsSortMode.None);

        //I need to exclude the mini map as a camera that needs to be faced
        foreach (var _targetCam in targetCameraArray)
        {
            if (!_targetCam.orthographic)
            {
                targetCameraList.Add(_targetCam);
            }
        }
    }

    void Update()
    {
        currentCamFollow = targetCameraList[0].transform;

        //Check which of the two cameras is nearer and face it
        if (Vector3.Distance(transform.position, currentCamFollow.transform.position) >
            Vector3.Distance(transform.position, targetCameraList[1].transform.position))
        {
            currentCamFollow = targetCameraList[1].transform;
        }

        var _rotation = currentCamFollow.transform.rotation;
            
        transform.LookAt(transform.position + _rotation * Vector3.forward, _rotation * Vector3.up);
    }
}
