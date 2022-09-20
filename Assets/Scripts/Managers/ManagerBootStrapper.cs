using UnityEngine;

namespace Managers
{
    public class ManagerBootStrapper : MonoBehaviour
    {
        public static void Execute()
        {
            Instantiate(Resources.Load("Prefabs/GameManager", typeof(GameObject)));
            Instantiate(Resources.Load("Prefabs/AudioManager", typeof(GameObject)));
            Debug.Log("GameManager and AudioManager created");
        }
    }
}