using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using TMPro;

namespace Gameplay
{
    public class BasicBallController : MonoBehaviour
    {
        public int force = 50;
        public GameObject trail;
        public float ballDuration = 5f;

        private Rigidbody2D _body;
        private Vector3 _startPos;
        private TrailRenderer _trailRenderer;
        private PowerBarController _powerBarController;
        private float _ballDurationTimer;
        private bool _ballTimerStarted;

        private bool _shoot;
        private Vector2 _shootDirection;
        private bool _resetBall;
        public static event Action<float> BallTimerChange;

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _trailRenderer = trail.GetComponent<TrailRenderer>();
            _body.constraints = RigidbodyConstraints2D.FreezePosition;
            _startPos = transform.position;
            _powerBarController = GetComponentInParent<PowerBarController>();
            _ballDurationTimer = ballDuration;
        }

        void OnCollisionExit2D()
        {
            _body.angularDrag = 100f;
        }

        // lower the drag for the ball to be able to roll on surfaces
        void OnCollisionStay2D()
        {
            _body.angularDrag = 0.05f;
        }

        // start ball duration timer on first collision
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_ballTimerStarted)
            {
                StartCoroutine(StartBallTimer());
            }
        }

        private void FixedUpdate()
        {
            if (_shoot)
            {
                GameManager.Instance.UpdateGameState(GameState.Shooting);
                _body.simulated = true;
                _body.constraints = RigidbodyConstraints2D.None;
                _body.AddForce(_shootDirection.normalized * force * _powerBarController.shotPowerModifier);
                _shoot = false;
            }

            if (_resetBall)
            {
                _ballTimerStarted = false;
                _ballDurationTimer = ballDuration;
                _body.constraints = RigidbodyConstraints2D.FreezePosition;
                transform.position = _startPos;
                transform.localRotation = Quaternion.identity;
                _body.simulated = false;
                _trailRenderer.Clear();
                _resetBall = false;
            }
        }

        private IEnumerator StartBallTimer()
        {
            _ballTimerStarted = true;
            while (_ballDurationTimer > 0)
            {
                BallTimerChange?.Invoke(_ballDurationTimer);
                _ballDurationTimer -= Time.deltaTime;
                yield return null;
            }
            
            BallTimerChange?.Invoke(0);
            ResetPos();
            CombatManager.Instance.DoCombat();
        }

        public void Shoot(Vector2 shootDirection)
        {
            _shoot = true;
            _shootDirection = shootDirection;
        }
        
        public void ResetPos()
        {
            _resetBall = true;
        }
    }
}
