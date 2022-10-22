using System;
using System.Collections;
using Gameplay.BallArena;
using Managers;
using UnityEngine;

namespace Gameplay.Balls
{
    public abstract class Ball : MonoBehaviour
    {
        public int force;
        public float ballDuration;
        public static event Action<float> BallTimerChange;
        public int explosionRadius;
        
        protected Rigidbody2D _body;
        protected Vector3 _startPos;
        protected TrailRenderer _trailRenderer;
        protected PowerBarController _powerBarController;
        protected float _ballDurationTimer;
        protected bool _ballTimerStarted;
        protected bool _firstCollision;
        protected bool _shoot;
        protected Vector2 _shootDirection;
        protected bool _resetBall;
        protected ParticleSystem _explosionParticle;

        protected virtual void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _trailRenderer = GetComponentInChildren<TrailRenderer>();
            _body.constraints = RigidbodyConstraints2D.FreezePosition;
            _startPos = transform.position;
            _powerBarController = GetComponentInParent<PowerBarController>();
            _ballDurationTimer = ballDuration;
            _explosionParticle = GetComponentInChildren<ParticleSystem>();
        }
        
        protected virtual void OnCollisionExit2D()
        {
            _body.angularDrag = 100f;
            if (_firstCollision)
            {
                FirstCollisionExit();
            }
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
                FirstCollisionEnter();
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

            ExplodeBall();
            BallTimerChange?.Invoke(0);
            StartResettingBallPosition();
            CombatManager.Instance.DoCombat();
        }
        
        protected virtual void Update()
        {
            if (_ballTimerStarted && Input.GetKeyDown(KeyCode.Space))
            {
                ExplodeBall();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (GameManager.Instance.state == GameState.Shooting)
            {
                transform.up = _body.velocity.normalized;
            }
            
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

        protected virtual void FirstCollisionEnter()
        {
            StartCoroutine(StartBallTimer());
            _firstCollision = true;
        }
        
        protected virtual void FirstCollisionExit()
        {
            _firstCollision = false;
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

        protected virtual void ExplodeBall()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            _explosionParticle.Play();
            _ballDurationTimer = 0;
            foreach(var collider in colliders)
            {
                var component = collider.GetComponent<APComponent>();
                if (component)
                {
                    component.ComponentHit();
                }
            }
        }
    }
}