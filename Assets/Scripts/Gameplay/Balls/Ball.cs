using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Gameplay.Balls
{
    public class Ball : MonoBehaviour
    {
        public int force;
        public float ballDuration;
        public static event Action<float> BallTimerChange;
        
        private Rigidbody2D _body;
        private Vector3 _startPos;
        private TrailRenderer _trailRenderer;
        private PowerBarController _powerBarController;
        private float _ballDurationTimer;
        private bool _ballTimerStarted;
        private bool _shoot;
        private Vector2 _shootDirection;
        private bool _resetBall;
        
        protected virtual void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _trailRenderer = GetComponentInChildren<TrailRenderer>();
            _body.constraints = RigidbodyConstraints2D.FreezePosition;
            _startPos = transform.position;
            _powerBarController = GetComponentInParent<PowerBarController>();
            _ballDurationTimer = ballDuration;
        }
        
        protected virtual void OnCollisionExit2D()
        {
            _body.angularDrag = 100f;
        }

        // lower the drag for the ball to be able to roll on surfaces
        protected virtual void OnCollisionStay2D()
        {
            _body.angularDrag = 0.05f;
        }

        // start ball duration timer on first collision
        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_ballTimerStarted)
            {
                FirstBallBounce();
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
            StartResettingBallPosition();
            CombatManager.Instance.DoCombat();
        }
        
        protected virtual void FixedUpdate()
        {
            if (_shoot)
            {
                ShootBall();
            }

            if (_resetBall)
            {
                _ballTimerStarted = false;
                _ballDurationTimer = ballDuration;
                ResetBallPosition();
            }
        }

        protected virtual void FirstBallBounce()
        {
            StartCoroutine(StartBallTimer());
        }
        
        protected virtual void ShootBall()
        {
            GameManager.Instance.UpdateGameState(GameState.Shooting);
            _body.constraints = RigidbodyConstraints2D.None;
            _body.AddForce(_shootDirection.normalized * force * _powerBarController.shotPowerModifier);
            _shoot = false;
        }

        protected virtual void ResetBallPosition()
        {
            _body.constraints = RigidbodyConstraints2D.FreezePosition;
            transform.position = _startPos;
            transform.localRotation = Quaternion.identity;
            _trailRenderer.Clear();
            _resetBall = false;
        }
        
        public void StartShooting(Vector2 shootDirection)
        {
            _shoot = true;
            _shootDirection = shootDirection;
        }
        
        public void StartResettingBallPosition()
        {
            _resetBall = true;
        }
    }
}