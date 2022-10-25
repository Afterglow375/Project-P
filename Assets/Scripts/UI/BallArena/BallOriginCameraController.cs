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
            Debug.Assert(_vCam.Follow != null, "Must set the Follow of BallOriginCameraController to be the ball");
            _vCam.Follow = null;
            gameObject.SetActive(false);
        }
    }
}