using UnityEngine;

namespace UI.Levels
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