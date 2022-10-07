using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Gameplay
{
    public class TriangleController : MonoBehaviour
    {
        [SerializeField] private int _points;
        [SerializeField] private int _force;
        public static event Action<int> TriangleHitEvent;
        private Animator _animator;
        private readonly int _triangleHit = Animator.StringToHash("BigPegHit");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                collision.rigidbody.AddForce(collision.GetContact(0).normal * _force, ForceMode2D.Impulse);
                _animator.SetTrigger(_triangleHit);
                CombatManager.Instance.SpawnDamageNumber(_points, transform);
                TriangleHitEvent?.Invoke(_points);
            }
        }
    }
}