using Managers;
using UnityEngine;

namespace Gameplay
{
    public class BallController : MonoBehaviour
    {
        public int force = 50;
        public GameObject trail;
    
        private Rigidbody2D _body;
        private Vector3 _startPos;
        private TrailRenderer _trailRenderer;

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _trailRenderer = trail.GetComponent<TrailRenderer>();
            _body.constraints = RigidbodyConstraints2D.FreezePosition;
            _startPos = transform.position;
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

        public void Shoot(Vector2 shootDirection)
        {
            GameManager.Instance.UpdateGameState(GameState.Shooting);
            _body.simulated = true;
            _body.constraints = RigidbodyConstraints2D.None;
            _body.AddForce(shootDirection.normalized * force);
        }
        
        public void ResetPos()
        {
            _body.velocity = Vector2.zero;
            transform.position = _startPos;
            transform.localRotation = Quaternion.identity;
            _body.simulated = false;
            _trailRenderer.Clear();
        }
    }
}
