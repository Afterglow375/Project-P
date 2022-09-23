using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
        
        private void Start()
        {
        }

        public void LoadLevel(string levelName)
        {
            Debug.Log("Level select state: " + GameManager.Instance.state);
            _sceneLoader.LoadScene(levelName);
        }
    }
}
