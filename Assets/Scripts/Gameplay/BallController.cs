using System;
using Managers;
using UnityEngine;
using TMPro;

namespace Gameplay
{
    public class BallController : MonoBehaviour
    {
        public int force = 50;
        public GameObject trail;
        public float ballDuration = 5f;
        public TMP_Text ballTimerText;

        private Rigidbody2D _body;
        private Vector3 _startPos;
        private TrailRenderer _trailRenderer;
        private PowerBarController _powerBarController;
        private float _ballDurationTimer;
        private bool _ballDurationTimerStarted;

        private bool _shoot;
        private Vector2 _shootDirection;
        private bool _resetBall;

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _trailRenderer = trail.GetComponent<TrailRenderer>();
            _body.constraints = RigidbodyConstraints2D.FreezePosition;
            _startPos = transform.position;
            _powerBarController = GetComponentInParent<PowerBarController>();
            _ballDurationTimer = ballDuration;
            ballTimerText.enabled = false;
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
            _ballDurationTimerStarted = true;
        }

        private void FixedUpdate()
        {
            ballTimerText.text = _ballDurationTimer.ToString("F1");

            if (_shoot)
            {
                GameManager.Instance.UpdateGameState(GameState.Shooting);
                _body.simulated = true;
                _body.constraints = RigidbodyConstraints2D.None;
                _body.AddForce(_shootDirection.normalized * force * _powerBarController.shotPowerModifier);
                ballTimerText.enabled = true;
                _shoot = false;
            }

            if (_resetBall)
            {
                _body.velocity = Vector2.zero;
                transform.position = _startPos;
                transform.localRotation = Quaternion.identity;
                _body.simulated = false;
                _trailRenderer.Clear();
                ballTimerText.enabled = false;
                _resetBall = false;
            }

            if (_ballDurationTimerStarted)
            {
                _ballDurationTimer -= Time.deltaTime;
                if (_ballDurationTimer <= 0)
                {
                    _ballDurationTimerStarted = false;
                    _ballDurationTimer = ballDuration;
                    ResetPos();
                    _body.constraints = RigidbodyConstraints2D.FreezePosition;
                    CombatManager.Instance.DoCombat();
                }
            }
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
