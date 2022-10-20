using System;
using Managers;
using UnityEngine;

namespace Gameplay.BallArena
{
    public class DiamondController : APComponent
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (IsBallCollided(col.gameObject))
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