using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    /// This class is responsible for the transition between scenes, it should be present in every scene.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        private float _transitionTime;
        public Animator transition;
        private readonly int _startCrossfade = Animator.StringToHash("Start Crossfade");
        
        private void Start()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            _transitionTime = transition.runtimeAnimatorController.animationClips[0].length;
        }

        public void LoadScene(string scene)
        {
            StartCoroutine(LoadSceneTransition(scene));
        }

        private IEnumerator LoadSceneTransition(string scene)
        {
            transition.SetTrigger(_startCrossfade);
            yield return new WaitForSeconds(_transitionTime);
            GameManager.Instance.UpdateGameState(GameState.LoadingScene);
            SceneManager.LoadScene(scene);
        }
    }
}
