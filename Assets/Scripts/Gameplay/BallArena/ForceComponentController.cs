using System;
using Managers;
using UnityEngine;

namespace Gameplay.BallArena
{
    public class ForceComponentController : APComponent
    {
        [SerializeField] private int _force;
        private Animator _animator;
        private readonly int _componentHit = Animator.StringToHash("ForceComponentHit");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public override void ComponentHit(Collision2D collision)
        {
            CombatManager.Instance.SpawnDamageNumber(_points, transform);
            InvokeHitEvent();
            collision.rigidbody.AddForce(collision.GetContact(0).normal * _force, ForceMode2D.Impulse);
            _animator.SetTrigger(_componentHit);
        }
    }
}