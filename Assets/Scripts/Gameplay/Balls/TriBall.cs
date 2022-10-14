namespace Gameplay.Balls
{
    /// <summary>
    /// TODO: TriBall splits into 3 smaller balls on first collision.
    /// </summary>
    public class TriBall : Ball
    {
        protected override void FirstCollisionExit()
        {
            base.FirstCollisionExit();
            transform.localScale *= .5f;
        }

        protected override void ResetBallPosition()
        {
            base.ResetBallPosition();
            transform.localScale *= 2f;
        }
    }
}
