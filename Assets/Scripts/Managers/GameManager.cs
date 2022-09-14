using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEditor;
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
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject gm = new GameObject("GameManager");
                    gm.AddComponent<GameManager>();
                    DontDestroyOnLoad(gm);
                    Debug.Log("Created GameManager");
                }

                return _instance;
            }
            private set => _instance = value;
        }
        
        public GameState State;

        void Awake()
        {
            // for safety, if there's a duplicate instance delete itself
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            // this works for setting initial GameState for now, but should probably be changed later
            UpdateStateBasedOnScene(SceneManager.GetActiveScene().name);
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
            State = newState;
        }

        public void ChangeScene(string scene)
        {
            UpdateGameState(GameState.LoadingScene);
            SceneManager.LoadScene(scene);
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
        ResettingBall,
        PlayerTurn,
        EnemyTurn,
        LevelVictory,
        NextLevel,
        GameVictory,
        LevelFailed,
        Pause
    }
}