using UnityEngine;

namespace UI.BallArena
{
    public class DamageNumber : MonoBehaviour
    {
        // used by animation event
        public void DestroyParent()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}