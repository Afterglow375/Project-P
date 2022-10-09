using System;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Managers
{
    /// <summary>
    /// The GameManager is a persistent singleton which stores high level data and handles GameState changes.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get; private set; }
        public GameState state;

        void Awake()
        {
            // for safety, delete duplicate instance if it exists in the scene
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void UpdateGameState(GameState newState)
        {
            Debug.Log("State changed: " + newState);
            state = newState;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string sceneName = scene.name;
            Debug.Log("Scene loaded: " + sceneName);
            UpdateStateBasedOnScene(sceneName);
        }

        private void UpdateStateBasedOnScene(string sceneName)
        {
            if (sceneName.Equals(Scenes.MainMenu))
            {
                UpdateGameState(GameState.MainMenu);
            }
            else if (sceneName.Equals(Scenes.LevelSelect))
            {
                UpdateGameState(GameState.LevelSelect);
            }
            else // we're in a level scene
            {
                UpdateGameState(GameState.ReadyToShoot);
            }
        }
    }

    public enum GameState
    {
        MainMenu,
        LevelSelect,
        LoadingScene,
        ReadyToShoot,
        Shooting,
        PlayerTurn,
        EnemyTurn,
        LevelVictory,
        GameVictory,
        LevelFailed,
        Pause
    }
}