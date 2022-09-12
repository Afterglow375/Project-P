using System;
using Managers;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class BallController : MonoBehaviour
    {
        public int force = 50;
        public GameObject trail;
    
        private Rigidbody2D _body;
        private Vector2 _startPos;
        private TrailRenderer _trailRenderer;

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _trailRenderer = trail.GetComponent<TrailRenderer>();
            _body.constraints = RigidbodyConstraints2D.FreezePosition;
            _startPos = _body.position;
        }

        private void OnTriggerEnter2D(Collider2D collision) // Hit reset trigger
        {
            if (collision.gameObject.CompareTag("Respawn"))
            {
                GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
            }
        }

        public void Shoot(Vector2 shootDirection)
        {
            _body.simulated = true;
            _body.constraints = RigidbodyConstraints2D.None;
            _body.AddForce(shootDirection.normalized * force);
            GameManager.Instance.UpdateGameState(GameState.Shooting);
        }
        
        public void ResetPos()
        {
            _body.velocity = Vector2.zero;
            _body.transform.position = _startPos;
            _body.simulated = false;
            _trailRenderer.Clear();
        }
    }
}
