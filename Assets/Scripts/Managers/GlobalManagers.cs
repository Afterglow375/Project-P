using UnityEngine;

namespace Managers
{
    /// <summary>
    /// The GlobalManagers is a persistent singleton which serves as the parent for all managers intended to be persistent.
    /// </summary>
    public class GlobalManagers : MonoBehaviour
    {
        private static GlobalManagers _instance;
        
        void Awake()
        {
            // for safety, delete duplicate instance if it exists in the scene
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
        }
    }
}