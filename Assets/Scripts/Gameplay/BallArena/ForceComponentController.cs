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

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                collision.rigidbody.AddForce(collision.GetContact(0).normal * _force, ForceMode2D.Impulse);
                _animator.SetTrigger(_componentHit);

                this.ComponentHit();
            }
        }
    }
}