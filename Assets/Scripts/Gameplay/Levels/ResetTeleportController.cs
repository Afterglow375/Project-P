using System;
using Managers;
using UnityEngine;

namespace Gameplay.Levels
{
    // Reset teleports are unused for now
    public class ResetTeleportController : MonoBehaviour
    {
        private Rigidbody2D _ballRigidBody;
        
        public static event Action ResetTeleportEvent;
        
        private void Start()
        {
            _ballRigidBody = GameObject.FindWithTag("Ball").GetComponent<Rigidbody2D>();
            Debug.Assert(_ballRigidBody != null);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ball") && GameManager.Instance.state == GameState.Shooting)
            {
                ResetTeleportEvent?.Invoke();
                _ballRigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
                CombatManager.Instance.DoCombat();
            }
        }
    }
}