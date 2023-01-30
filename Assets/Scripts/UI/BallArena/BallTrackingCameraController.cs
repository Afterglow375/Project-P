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
        [SerializeField] private Collider2D _cameraBounds;
        // cameraSensitvity controls how fast camera reaches full speed when starting to move
        [SerializeField] private float _cameraSensitivity = 10;
        // cameraSensitvity controls how fast camera stops moving after stopping camera movement
        [SerializeField] private float _cameraGravity = 5;
        [SerializeField] private float _zoomMultiplier = 2;
        
        private CinemachineVirtualCamera _vCam;
        private Transform _ballTransform;
        private CinemachineFramingTransposer _composer;
        private Vector3 _origCameraPosition;
        private float _origDeadZoneWidth;
        private float _origDeadZoneHeight;
        private float _horizontalAxis;
        private float _verticalAxis;
        private float _zoomChange;
        private bool _cameraChange;
        private Vector3 _mouseDragOrigin;
        private Vector3 _panDirection = Vector3.zero;
        private Camera _cam;

        private float _cameraBoundsMinX;
        private float _cameraBoundsMaxX;
        private float _cameraBoundsMinY;
        private float _cameraBoundsMaxY;
        private float _maxOrthographicSize;

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
            SetupCameraConfines();
        }

        private void SetupCameraConfines()
        {
            Debug.Assert(_cameraBounds != null, "Must set the camera confines collider");
            _cameraBoundsMinX = _cameraBounds.bounds.min.x;
            _cameraBoundsMaxX = _cameraBounds.bounds.max.x;
            _cameraBoundsMinY = _cameraBounds.bounds.min.y;
            _cameraBoundsMaxY = _cameraBounds.bounds.max.y;

            float confinesWidth = _cameraBounds.bounds.size.x;
            float confinesHeight = _cameraBounds.bounds.size.y;
            _maxOrthographicSize = Mathf.Min(confinesHeight / 2, confinesWidth / 2 / _cam.aspect);
            
            // delete camera bounds collider to avoid physics bugs
            Destroy(_cameraBounds.gameObject);
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
                HandleCameraPan();
                HandleCameraZoom();
                HandleCameraReset();
                ClampCamera();
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

        private void HandleCameraPan()
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
                _cameraChange = true;
            }
            else // camera panning w/ WASD or edge panning
            { 
                _horizontalAxis = Input.GetAxisRaw("Horizontal");
                _verticalAxis = Input.GetAxisRaw("Vertical");
                // TODO: uncomment this for release
                // CalculateEdgePanDirection(Input.mousePosition);
                _panDirection.x = GetSmoothAxis(_horizontalAxis, _panDirection.x);
                _panDirection.y = GetSmoothAxis(_verticalAxis, _panDirection.y);
                if (_panDirection != Vector3.zero)
                {
                    _vCam.Follow = null;
                    // clamp transform position to be within confines
                    transform.position += _panDirection * _cameraMoveSpeed * Time.deltaTime;
                    _cameraChange = true;
                }
            }
        }
        
        // zoom in/out with mousewheel
        private void HandleCameraZoom()
        {
            _zoomChange = Input.GetAxis("Mouse ScrollWheel") * _zoomMultiplier;
            if (_zoomChange != 0)
            {
                float zoomChange = _vCam.m_Lens.OrthographicSize;
                _vCam.m_Lens.OrthographicSize -= _zoomChange;
                _vCam.m_Lens.OrthographicSize = Mathf.Clamp(_vCam.m_Lens.OrthographicSize, 2f, _maxOrthographicSize);
                zoomChange -= _vCam.m_Lens.OrthographicSize;
                if (zoomChange != 0)
                {
                    // have to shift camera down slightly because of combat HUD
                    _composer.ForceCameraPosition(transform.position + new Vector3(0f, -0.25f * zoomChange, 0f), Quaternion.identity);
                    _cameraChange = true;
                }
            }
        }

        // C to reset cam to follow ball
        private void HandleCameraReset()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Input.ResetInputAxes();
                _panDirection = Vector3.zero;
                _vCam.Follow = _ballTransform;
                _cameraChange = false;
            }
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

        // clamp camera to be within confines
        private void ClampCamera()
        {
            if (!_cameraChange) return;
            
            float camHeight = _vCam.m_Lens.OrthographicSize;
            float camWidth = _vCam.m_Lens.OrthographicSize * _vCam.m_Lens.Aspect;
            float minX = _cameraBoundsMinX + camWidth;
            float maxX = _cameraBoundsMaxX - camWidth;
            float minY = _cameraBoundsMinY + camHeight;
            float maxY = _cameraBoundsMaxY - camHeight;
            float newX = Mathf.Clamp(transform.position.x, minX, maxX);
            float newY = Mathf.Clamp(transform.position.y, minY, maxY);
            
            transform.position = new Vector3(newX, newY, transform.position.z);
            _cameraChange = false;
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