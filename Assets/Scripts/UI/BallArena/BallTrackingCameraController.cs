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
        private Transform _ballTransform;
        private CinemachineFramingTransposer _composer;
        private Vector3 _origCameraPosition;
        private float _origDeadZoneWidth;
        private float _origDeadZoneHeight;
        private bool _resetBall;

        private void Start()
        {
            Ball.BallExplosionEvent += BallExplosionEvent;
            LauncherController.BallSwitched += BallSwitched;
            _vCam = GetComponent<CinemachineVirtualCamera>();
            Debug.Assert(_vCam.Follow != null, "Must set the Follow of BallTrackingCameraController to be the ball");
            _composer = _vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            _origDeadZoneWidth = _composer.m_DeadZoneWidth;
            _origDeadZoneHeight = _composer.m_DeadZoneHeight;
            _ballTransform = _vCam.Follow;
        }

        private void OnDestroy()
        {
            Ball.BallExplosionEvent -= BallExplosionEvent;
            LauncherController.BallSwitched -= BallSwitched;
        }
        
        void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                _vCam.Follow = null;
                Vector3 direction = new Vector3(horizontal, vertical, 0);
                Debug.Log(direction);
                transform.position += direction * _cameraMoveSpeed * Time.deltaTime;
            }
            
            if (Input.GetKeyDown(KeyCode.C))
            {
                Input.ResetInputAxes();
                _vCam.Follow = _ballTransform;
            }
            
            float zoom = Input.GetAxis("Mouse ScrollWheel");

            if (zoom != 0)
            {
                // prevent zoom in past 1.0f orthographic cam size
                if (zoom > 0 && _vCam.m_Lens.OrthographicSize < 1.01f) return;

                _vCam.m_Lens.OrthographicSize -= zoom;
                _composer.ForceCameraPosition(transform.position + new Vector3(0f, -0.25f * zoom, 0f), Quaternion.identity);
            }
            
            // Check if the middle mouse button is pressed
            if (Input.GetMouseButton(2))
            {
                // Pan the camera based on the mouse movement
                Debug.Log(Input.GetAxis("Mouse X") + " " + Input.GetAxis("Mouse Y"));
                // _vCam.Pan(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }

            Vector2 panDirection = PanDirection(Input.mousePosition);
            if (panDirection != Vector2.zero)
            {
                _vCam.Follow = null;
                transform.position += (Vector3) panDirection * _cameraMoveSpeed * Time.deltaTime;
            }
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
            _vCam.Follow = _ballTransform;
        }

        private Vector2 PanDirection(Vector2 mousePos)
        {
            Vector2 direction = Vector2.zero;
            if (mousePos.y >= Screen.height * .99f)
            {
                direction.y += 1;
            }
            else if (mousePos.y <= Screen.height * .01f)
            {
                direction.y -= 1;
            }
            if (mousePos.x >= Screen.width * .99f)
            {
                direction.x += 1;
            }
            else if (mousePos.x <= Screen.width * .01f)
            {
                direction.x -= 1;
            }

            return direction;
        }

        private void BallSwitched(Ball ball)
        {
            _ballTransform = _vCam.Follow = ball.transform;
        }

        public float GetCamOrthoSize()
        {
            return _vCam.m_Lens.OrthographicSize;
        }
    }
}