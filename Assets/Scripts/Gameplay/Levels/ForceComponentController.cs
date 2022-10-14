using System;
using Managers;
using UnityEngine;

namespace Gameplay.Levels
{
    public class ForceComponentController : MonoBehaviour
    {
        [SerializeField] private int _points;
        [SerializeField] private int _force;
        public static event Action<int> ComponentHitEvent;
        private Animator _animator;
        private readonly int _componentHit = Animator.StringToHash("ForceComponentHit");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                collision.rigidbody.AddForce(collision.GetContact(0).normal * _force, ForceMode2D.Impulse);
                _animator.SetTrigger(_componentHit);
                CombatManager.Instance.SpawnDamageNumber(_points, transform);
                ComponentHitEvent?.Invoke(_points);
            }
        }
    }
}