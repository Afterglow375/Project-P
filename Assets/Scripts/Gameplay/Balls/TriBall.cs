using System;
using UnityEngine;

namespace Gameplay.Balls
{
    public class TriBall : Ball
    {
        // the first bounce of the TriBall splits it into 3 smaller ones
        protected override void FirstBallBounce()
        {
            base.FirstBallBounce();
        }
    }
}
