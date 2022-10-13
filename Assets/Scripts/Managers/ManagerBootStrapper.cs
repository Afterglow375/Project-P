using UnityEngine;

namespace Managers
{
    public class ManagerBootStrapper : MonoBehaviour
    {
        public static void Execute()
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            Instantiate(Resources.Load<GameObject>("Prefabs/AudioManager"));
            Debug.Log("GameManager and AudioManager created");
        }
    }
}