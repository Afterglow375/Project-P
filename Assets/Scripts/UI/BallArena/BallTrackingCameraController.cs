using System;
using Cinemachine;
using Gameplay.Balls;
using Managers;
using UnityEngine;

namespace UI.BallArena
{
    public class BallTrackingCameraController : MonoBehaviour
    {
        [SerializeField] private float _cameraMoveSpeed;
        
        private CinemachineVirtualCamera _vCam;
        private CinemachineFramingTransposer _composer;
        private Vector3 _origCameraPosition;
        private float _origDeadZoneWidth;
        private float _origDeadZoneHeight;

        private void Start()
        {
            Ball.BallExplosionEvent += BallExplosionEvent;
            LauncherController.BallSwitched += BallSwitched;
            _vCam = GetComponent<CinemachineVirtualCamera>();
            Debug.Assert(_vCam.Follow != null, "Must set the Follow of BallTrackingCameraController to be the ball");
            _composer = _vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            _origDeadZoneWidth = _composer.m_DeadZoneWidth;
            _origDeadZoneHeight = _composer.m_DeadZoneHeight;
        }

        private void OnDestroy()
        {
            Ball.BallExplosionEvent -= BallExplosionEvent;
            LauncherController.BallSwitched -= BallSwitched;
        }

        // center on ball when it explodes via removing dead zone
        private void BallExplosionEvent()
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

        private void BallSwitched(Ball ball)
        {
            _vCam.Follow = ball.transform;
        }
        
        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                _vCam.Follow = null;
                Vector3 direction = new Vector3(horizontal, vertical, 0);
                transform.position += direction * _cameraMoveSpeed * Time.deltaTime;
            }
        }
    }
}