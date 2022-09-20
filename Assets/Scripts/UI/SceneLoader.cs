using System;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// The class is responsible for the transition between scenes, it should be present in every scene.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        // private static SceneLoader _instance;
        // public static SceneLoader Instance { get; private set; }
        
        public float transitionTime = 0.5f;
        public Animator transition;
        private readonly int _startCrossfade = Animator.StringToHash("Start Crossfade");
        
        void Awake()
        {
            // // for safety, if there's a duplicate instance delete itself
            // if (_instance != null && _instance != this)
            // {
            //     Destroy(gameObject);
            // }
            // else
            // {
            //     Instance = this;
            // }
        }

        private void Start()
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        public void LoadScene(string scene)
        {
            StartCoroutine(LoadSceneTransition(scene));
        }

        private IEnumerator LoadSceneTransition(string scene)
        {
            transition.SetTrigger(_startCrossfade);
            yield return new WaitForSeconds(transitionTime);
            GameManager.Instance.UpdateGameState(GameState.LoadingScene);
            SceneManager.LoadScene(scene);
        }
    }
}
