using System.Diagnostics;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace UI
{
    public class LevelSelector : MonoBehaviour
    {
        public void LoadLevel(string levelName)
        {
            GameManager.Instance.ChangeScene(levelName);
        }
    }
}
