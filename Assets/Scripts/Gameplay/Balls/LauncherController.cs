using System;
using Managers;
using UnityEngine;

namespace Gameplay.Balls
{
    public class LauncherController : MonoBehaviour
    {
        private Vector2 _shootDirection;
        private Vector3 _mousePos;
        private Camera _camera;
        private GameObject _ballPrefab;
        private Vector3 _ballLocation;
        private Ball _ball;
        
        // params: ball duration
        public static event Action<float> BallSwitched;
        
        private void Start()
        {
            _camera = Camera.main;
            _ballPrefab = transform.GetChild(0).gameObject;
            _ballLocation = _ballPrefab.transform.position;
            _ball = GetComponentInChildren<Ball>();
        }

        void Update()
        {
            if (GameManager.Instance.state == GameState.ReadyToShoot)
            {
                _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                _shootDirection = (_mousePos - transform.position).normalized;
                transform.up = _shootDirection;
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _ball.StartShooting(_shootDirection);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1) && _ball is not BasicBall)
                {
                    SwitchBall("BasicBall");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) && _ball is not TriBall)
                {
                    SwitchBall("TriBall");
                }
            }
        }

        private void SwitchBall(string ball)
        {
            Quaternion rotation = _ballPrefab.transform.localRotation;
            Destroy(_ballPrefab);
            _ballPrefab = Instantiate(Resources.Load<GameObject>($"Prefabs/Balls/{ball}"), transform, true);
            _ballPrefab.transform.position = _ballLocation;
            _ballPrefab.transform.localRotation = rotation;
            _ball = _ballPrefab.GetComponent<Ball>();
            BallSwitched?.Invoke(_ball.ballDuration);
            Debug.Log("Ball switched: " + ball);
        }
    }
}
