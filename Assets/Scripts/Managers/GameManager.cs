using System;
using System.Collections.Generic;
using Gameplay;
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
        private Vector2 _levelSelectPosition = new Vector2(0, -2);
        private Dictionary<string, Level> _levels = new();
        private bool _levelsInitialized;
        private string _lastLvl;

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

        // this method is called AFTER Awake but BEFORE Start
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
                SetRubyPosition();
                SetupLevels();
                UpdateGameState(GameState.LevelSelect);
            }
            else // we're in a level scene
            {
                UpdateGameState(GameState.ReadyToShoot);
            }
        }

        public void SaveRubyPosition(Vector2 position)
        {
            _levelSelectPosition = position;
        }

        private void SetRubyPosition()
        {
            RubyController ruby = GameObject.Find("Ruby").GetComponent<RubyController>();
            ruby.transform.position = _levelSelectPosition;
        }

        // TODO this func should read from save file
        private void SetupLevels()
        {
            if (!LevelsInitialized())
            {
                foreach (var level in _levels.Values)
                {
                    if (level.lastLvl)
                    {
                        _lastLvl = level.name;
                    }
                    
                    foreach (var nextLevel in level.nextLevelNames)
                    {
                        level.AddNextLevel(_levels[nextLevel]);
                    }
                }

                _levelsInitialized = true;
            }
        }

        public void LevelCompleted(string levelName)
        {
            _levels[levelName].LevelCompleted();
        }
        
        public void AddLevel(Level level)
        {
            _levels.Add(level.name, level);
        }
        
        public LevelStatus GetLevelStatus(string levelName)
        {
            return _levels[levelName].status;
        }

        public bool LevelsInitialized()
        {
            return _levelsInitialized;
        }

        public bool IsLastLevel()
        {
            return _lastLvl == SceneManager.GetActiveScene().name;
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