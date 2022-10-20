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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsBallCollided(collision.gameObject))
            {
                ComponentHit(collision);
            }
        }

        protected override void ComponentHit(Collision2D collision)
        {
            base.ComponentHit(collision);
            collision.rigidbody.AddForce(collision.GetContact(0).normal * _force, ForceMode2D.Impulse);
            _animator.SetTrigger(_componentHit);
        }
    }
}