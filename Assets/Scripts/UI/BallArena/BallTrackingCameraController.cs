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
        private float _zoom;
        private Vector3 _mouseDragOrigin;
        private Vector3 _panDirection = Vector3.zero;
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
                // camera panning when holding middle mouse and dragging
                if (Input.GetMouseButtonDown(2))
                {
                    _vCam.Follow = null;
                    _panDirection = Vector3.zero;
                    _mouseDragOrigin = _cam.ScreenToWorldPoint(Input.mousePosition);
                }
                if (Input.GetMouseButton(2))
                {
                    Vector3 difference = _mouseDragOrigin - _cam.ScreenToWorldPoint(Input.mousePosition);
                    transform.position += difference;
                }
                else
                {
                    _horizontalAxis = Input.GetAxisRaw("Horizontal");
                    _verticalAxis = Input.GetAxisRaw("Vertical");
                    // TODO: uncomment this for release
                    // CalculateEdgePanDirection(Input.mousePosition);
                    _panDirection.x = GetSmoothAxis(_horizontalAxis, _panDirection.x);
                    _panDirection.y = GetSmoothAxis(_verticalAxis, _panDirection.y);
                    
                    // camera panning w/ WASD or edge panning
                    if (_panDirection != Vector3.zero)
                    {
                        _vCam.Follow = null;
                        transform.position += _panDirection * _cameraMoveSpeed * Time.deltaTime;
                    }
                }

                // zoom in/out with mousewheel
                _zoom = Input.GetAxis("Mouse ScrollWheel") * 2;
                if (_zoom != 0)
                {
                    float zoomChange = _vCam.m_Lens.OrthographicSize;
                    _vCam.m_Lens.OrthographicSize -= _zoom;
                    _vCam.m_Lens.OrthographicSize = Mathf.Clamp(_vCam.m_Lens.OrthographicSize, 2f, 20f);
                    zoomChange -= _vCam.m_Lens.OrthographicSize;
                    if (zoomChange != 0)
                    {
                        _composer.ForceCameraPosition(transform.position + new Vector3(0f, -0.25f * zoomChange, 0f), Quaternion.identity);
                    }
                }
                
                // C to reset cam
                if (Input.GetKeyDown(KeyCode.C))
                {
                    Input.ResetInputAxes();
                    _panDirection = Vector3.zero;
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
        
        private float GetSmoothAxis(float r, float axis, float limit = 1f)
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

            return axis;
        }

        private void CalculateEdgePanDirection(Vector2 mousePos)
        {
            if (mousePos.y >= Screen.height * .999f && _verticalAxis < 1)
            {
                _verticalAxis += 1;
            }
            else if (mousePos.y <= .001f && _verticalAxis > -1)
            {
                _verticalAxis -= 1;
            }
            if (mousePos.x >= Screen.width * .999f && _horizontalAxis < 1)
            {
                _horizontalAxis += 1;
            }
            else if (mousePos.x <= .001f && _horizontalAxis > -1)
            {
                _horizontalAxis -= 1;
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