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

        public void AdjustCameraPosition(float orthoSize)
        {
            float oldOrthoSize = _vCam.m_Lens.OrthographicSize;
            Vector3 adjustment = new Vector3(0f, (orthoSize - oldOrthoSize) * 0.25f, 0f);
            _vCam.m_Lens.OrthographicSize = orthoSize;
            _vCam.ForceCameraPosition(transform.position + adjustment, Quaternion.identity);
        }
    }
}