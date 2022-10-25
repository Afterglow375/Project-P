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
        public static event Action BallExplosion;
        public int explosionRadius;
        
        protected Rigidbody2D _body;
        protected Vector3 _startPos;
        protected TrailRenderer _trailRenderer;
        protected SpriteRenderer _spriteRenderer;
        protected PowerBarController _powerBarController;
        protected float _ballDurationTimer;
        protected bool _ballTimerStarted;
        protected bool _firstCollision;
        protected bool _shoot;
        protected bool _onCollisionStay;
        protected Vector2 _shootDirection;
        protected bool _resetBall;
        protected ParticleSystem _explosionParticle;
        protected float _explosionDuration;

        private Color _origColor;
        private Color _transparentColor;

        protected virtual void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _origColor = _transparentColor = _spriteRenderer.color;
            _transparentColor.a = 0f;
            _trailRenderer = GetComponentInChildren<TrailRenderer>();
            _body.constraints = RigidbodyConstraints2D.FreezePosition;
            _startPos = transform.position;
            _powerBarController = GetComponentInParent<PowerBarController>();
            _ballDurationTimer = ballDuration;
            _explosionParticle = GetComponentInChildren<ParticleSystem>();
            _explosionDuration = _explosionParticle.main.duration;
            
            CombatManager.CombatEndEvent += TriggerBallReset;
        }

        private void OnDestroy()
        {
            CombatManager.CombatEndEvent += TriggerBallReset;
        }

        protected virtual void OnCollisionExit2D()
        {
            _onCollisionStay = false;
            if (_firstCollision)
            {
                FirstCollisionExit();
            }
        }

        // lower the drag for the ball to be able to roll on surfaces
        protected virtual void OnCollisionStay2D()
        {
            _onCollisionStay = true;
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

            yield return ExplodeBall();
        }

        protected virtual void Update()
        {
            if (_ballTimerStarted && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(ExplodeBall());
            }
        }

        protected virtual void FixedUpdate()
        {
            if (GameManager.Instance.state == GameState.Shooting && !_onCollisionStay)
            {
                _body.angularVelocity = 0;
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
            ShowBall();
            _body.constraints = RigidbodyConstraints2D.FreezePosition;
            transform.position = _startPos;
            transform.localRotation = Quaternion.identity;
            _trailRenderer.Clear();
            _resetBall = false;
        }
        
        public void TriggerShoot(Vector2 shootDirection)
        {
            _shoot = true;
            _shootDirection = shootDirection;
        }
        
        public void TriggerBallReset()
        {
            GameManager.Instance.UpdateGameState(GameState.ResettingBall);
            _resetBall = true;
        }

        private void HideBall()
        {
            _spriteRenderer.color = _transparentColor;
        }

        private void ShowBall()
        {
            _spriteRenderer.color = _origColor;
        }

        private IEnumerator ExplodeBall()
        {
            _ballDurationTimer = 0;
            BallTimerChange?.Invoke(_ballDurationTimer);
            GameManager.Instance.UpdateGameState(GameState.BallExploding);
            _body.constraints = RigidbodyConstraints2D.FreezeAll;
            HideBall();
            BallExplosion?.Invoke();
            _explosionParticle.Play();
            var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (var collider in colliders)
            {
                var component = collider.GetComponent<APComponent>();
                if (component)
                {
                    component.ComponentHit();
                }
            }
            
            yield return new WaitForSeconds(_explosionDuration);
            CombatManager.Instance.DoCombat();
        }
    }
}