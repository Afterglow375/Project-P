using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Managers
{
    /// <summary>
    /// Responsible for the transition between scenes
    /// </summary>
    public class SceneLoader : PersistentSingleton<SceneLoader>
    {
        [SerializeField] private GameObject _canvasObject;
        [SerializeField] private CanvasGroup _canvasGroup;
        private float _transitionTime = .25f;
        private readonly int _sceneFadeout = Animator.StringToHash("Scene Fadeout");
        private readonly int _sceneFadein = Animator.StringToHash("Scene Fadein");
        
        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            FadeIn();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneTransition(sceneName));
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)
            {
                FadeIn();
            }
        }

        // when the scene change ends
        private void FadeIn()
        {
            _canvasGroup.DOFade(0f, _transitionTime).SetEase(Ease.OutSine).OnComplete(() =>
            {
                _canvasObject.SetActive(false);
            });
        }
        
        // when the scene change starts
        private Tween FadeOut()
        {
            _canvasObject.SetActive(true);
            return _canvasGroup.DOFade(1f, _transitionTime).SetEase(Ease.OutSine);
        }
        
        private IEnumerator LoadSceneTransition(string sceneName)
        {
            GameManager.Instance.UpdateGameState(GameState.LoadingScene);
            // start loading scene asynchronously but don't change scene immediately
            AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;
            // wait for fadeout animation to complete before changing scene
            yield return FadeOut().WaitForCompletion();
            scene.allowSceneActivation = true;
        }
    }
}
