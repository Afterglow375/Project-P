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
        private float _cameraSensitivity = 10;
        private float _cameraGravity = 5;
        private float _horizontalAxis;
        private float _verticalAxis;
        private Vector3 _mouseDragOrigin;
        private Vector2 _panDirection = Vector2.zero;
        private Camera _cam;

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
            _cam = Camera.main;
        }

        private void OnDestroy()
        {
            Ball.BallExplosionEvent -= BallExplosionEvent;
            LauncherController.BallSwitched -= BallSwitched;
        }
        
        void Update()
        {
            if (GameManager.Instance.state != GameState.ResettingBall && GameManager.Instance.state != GameState.Pause)
            {
                _panDirection.x = Input.GetAxisRaw("Horizontal");
                _panDirection.y = Input.GetAxisRaw("Vertical");
                // TODO: uncomment this for release
                // CalculateEdgePanDirection(Input.mousePosition);
                GetSmoothRawAxis(_panDirection.x, ref _horizontalAxis);
                GetSmoothRawAxis(_panDirection.y, ref _verticalAxis);
                
                // camera panning when holding middle mouse and dragging
                if (Input.GetMouseButtonDown(2))
                {
                    _vCam.Follow = null;
                    _mouseDragOrigin = _cam.ScreenToWorldPoint(Input.mousePosition);
                }
                if (Input.GetMouseButton(2))
                {
                    Vector3 difference = _mouseDragOrigin - _cam.ScreenToWorldPoint(Input.mousePosition);
                    transform.position += difference;
                }
                else if (_horizontalAxis != 0 || _verticalAxis != 0)
                {
                    _vCam.Follow = null;
                    Vector3 direction = new Vector3(_horizontalAxis, _verticalAxis, 0);
                    transform.position += direction * _cameraMoveSpeed * Time.deltaTime;
                }

                // zoom in/out with mousewheel
                float zoom = Input.GetAxis("Mouse ScrollWheel");
                if (zoom != 0)
                {
                    // prevent zoom in past 1.0f orthographic cam size
                    _vCam.m_Lens.OrthographicSize -= zoom;
                    _vCam.m_Lens.OrthographicSize = Mathf.Clamp(_vCam.m_Lens.OrthographicSize, 1f, 15f);
                    // _composer.ForceCameraPosition(transform.position + new Vector3(0f, -0.25f * zoom, 0f), Quaternion.identity);
                }
                
                // C to reset cam
                if (Input.GetKeyDown(KeyCode.C))
                {
                    Input.ResetInputAxes();
                    _horizontalAxis = 0;
                    _verticalAxis = 0;
                    _vCam.Follow = _ballTransform;
                }
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
        
        private void GetSmoothRawAxis(float r, ref float axis, float limit = 1f)
        {
            if (r != 0)
            {
                if (Mathf.Sign(r) != Mathf.Sign(axis))
                {
                    axis = 0;
                }
                axis = Mathf.Clamp(axis + r * _cameraSensitivity * Time.deltaTime, -limit, limit);
            }
            else
            {
                axis = Mathf.Clamp01(Mathf.Abs(axis) - _cameraGravity * Time.deltaTime) * Mathf.Sign(axis);
            }
        }

        private void CalculateEdgePanDirection(Vector2 mousePos)
        {
            if (mousePos.y >= Screen.height * .999f && _panDirection.y < 1)
            {
                _panDirection.y += 1;
            }
            else if (mousePos.y <= .001f && _panDirection.y > -1)
            {
                _panDirection.y -= 1;
            }
            if (mousePos.x >= Screen.width * .999f && _panDirection.x < 1)
            {
                _panDirection.x += 1;
            }
            else if (mousePos.x <= .001f && _panDirection.x > -1)
            {
                _panDirection.x -= 1;
            }
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