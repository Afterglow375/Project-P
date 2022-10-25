using System;
using Cinemachine;
using UnityEngine;

namespace UI.BallArena
{
    public class BallOriginCameraController : MonoBehaviour
    {
        private CinemachineVirtualCamera _vCam;
        
        private void Start()
        {
            _vCam = GetComponent<CinemachineVirtualCamera>();
            Debug.Assert(_vCam.Follow != null);
            _vCam.Follow = null;
            gameObject.SetActive(false);
        }
    }
}