using System.Collections.Generic;
using Gameplay.Overworld;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Managers
{
    /// <summary>
    /// The GameManager is a persistent singleton which stores high level data and handles GameState changes.
    /// </summary>
    public class GameManager : PersistentSingleton<GameManager>
    {
        public GameState state;
        private Vector2 _overworldPosition = new Vector2(0, -2);
        private Dictionary<string, Level> _levels = new();
        private bool _levelsInitialized;
        private string _lastLvl;

        private void Start()
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
            string sceneMode = mode == LoadSceneMode.Single ? "as single" : "additively";
            Debug.Log($"Scene loaded {sceneMode}: {sceneName}");
            UpdateStateBasedOnScene(sceneName);
        }

        private void UpdateStateBasedOnScene(string sceneName)
        {
            if (sceneName.Equals(Scenes.MainMenu))
            {
                UpdateGameState(GameState.MainMenu);
            }
            else if (sceneName.Equals(Scenes.Overworld))
            {
                SetRubyPosition();
                SetupLevels();
                UpdateGameState(GameState.Overworld);
            }
            else // we're in a level scene
            {
                UpdateGameState(GameState.ReadyToShoot);
            }
        }

        public void SaveRubyPosition(Vector2 position)
        {
            _overworldPosition = position;
        }

        private void SetRubyPosition()
        {
            RubyController ruby = GameObject.Find("Ruby").GetComponent<RubyController>();
            ruby.transform.position = _overworldPosition;
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
        Overworld,
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