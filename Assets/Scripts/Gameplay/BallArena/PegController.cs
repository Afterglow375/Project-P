using System;
using Managers;
using UnityEngine;

namespace Gameplay.BallArena
{
    public class PegController : APComponent
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsBallCollided(collision.gameObject))
            {
                ComponentHit();
            }
        }

        public override void ComponentHit()
        {
            base.ComponentHit();
            gameObject.SetActive(false);
        }
    }
}
