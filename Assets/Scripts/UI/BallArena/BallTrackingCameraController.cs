using System;
using Cinemachine;
using Gameplay.Balls;
using Managers;
using UnityEngine;

namespace UI.BallArena
{
    public class BallTrackingCameraController : MonoBehaviour
    {
        private CinemachineVirtualCamera _vCam;
        private CinemachineFramingTransposer _composer;
        private Vector3 _origCameraPosition;
        private float _origDeadZoneWidth;
        private float _origDeadZoneHeight;

        private void Start()
        {
            Ball.BallExplosion += BallExplosion;
            _vCam = GetComponent<CinemachineVirtualCamera>();
            Debug.Assert(_vCam.Follow != null);
            _composer = _vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            _origDeadZoneWidth = _composer.m_DeadZoneWidth;
            _origDeadZoneHeight = _composer.m_DeadZoneHeight;
        }

        private void OnDestroy()
        {
            Ball.BallExplosion -= BallExplosion;
        }

        // center on ball when it explodes via removing dead zone
        private void BallExplosion()
        {
            _composer.m_DeadZoneWidth = 0f;
            _composer.m_DeadZoneHeight = 0f;
            _composer.m_UnlimitedSoftZone = true;
        }

        public void ResetBallSettings()
        {
            _composer.m_DeadZoneWidth = _origDeadZoneWidth;
            _composer.m_DeadZoneHeight = _origDeadZoneHeight;
            _composer.m_UnlimitedSoftZone = false;       
        }
    }
}