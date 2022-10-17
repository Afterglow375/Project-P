using System;
using Managers;
using UnityEngine;

namespace Gameplay.BallArena
{
    public class DiamondController : APComponent
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            ComponentHit();
        }
    }
}