using UnityEngine;

namespace Managers
{
    public class ManagerBootStrapper : MonoBehaviour
    {
        public void Execute()
        {
            string prefix = "Prefabs/Global Objects/";
            Instantiate(Resources.Load<GameObject>(prefix + "GameManager"));
            Instantiate(Resources.Load<GameObject>(prefix + "AudioManager"));
            Instantiate(Resources.Load<GameObject>(prefix + "SceneLoader"));
            Debug.Log("GameManager, AudioManager, SceneLoader created");
            // destroy since we no longer need it
            Destroy(this);
        }
    }
}