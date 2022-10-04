using UnityEngine;

namespace UI
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