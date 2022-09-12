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
        private BallController _ballController;

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
            
            switch (newState)
            {
                case GameState.MainMenu:
                    HandleMainMenu();
                    break;
                case GameState.LevelSelect:
                    HandleLevelSelect();
                    break;
                case GameState.LoadingScene:
                    HandleLoadingScene();
                    break;
                case GameState.ReadyToShoot:
                    HandleWaitingToShoot();
                    break;
                case GameState.Shooting:
                    HandleShooting();
                    break;
                case GameState.ResettingBall:
                    HandleResettingBall();
                    break;
                case GameState.PlayerTurn:
                    HandlePlayerTurn();
                    break;
                case GameState.EnemyTurn:
                    HandleEnemyTurn();
                    break;
                case GameState.LevelVictory:
                    HandleLevelVictory();
                    break;
                case GameState.NextLevel:
                    HandleNextLevel();
                    break;
                case GameState.GameVictory:
                    HandleGameVictory();
                    break;
                case GameState.Lose:
                    HandleLose();
                    break;
                case GameState.Pause:
                    HandlePause();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        private void HandleMainMenu()
        {
                    
        }
        
        private void HandleLevelSelect()
        {
        
        }
    
        private void HandleLoadingScene()
        {   
            // TODO: loading screen?
        }
    
        private void HandleWaitingToShoot()
        {
        
        }

        private void HandleShooting()
        {
            
        }
        
        private void HandleResettingBall()
        {
            _ballController.ResetPos();
            CombatManager.Instance.ResetPegScore();
            _instance.UpdateGameState(GameState.ReadyToShoot);
        }
    
        private void HandlePlayerTurn()
        {
            CombatManager.Instance.PlayerTurn();
            _instance.UpdateGameState(GameState.EnemyTurn);
        }
    
        private void HandleEnemyTurn()
        {
            CombatManager.Instance.EnemyTurn();
            _instance.UpdateGameState(GameState.ResettingBall);
        }

        private void HandleGameVictory()
        {
        
        }

        private void HandleLevelVictory()
        {
        
        }
        
        private void HandleNextLevel()
        {
        
        }

        private void HandleLose()
        {
            
        }
    
        private void HandlePause()
        {
            
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
                _ballController = GameObject.FindWithTag("Ball").GetComponent<BallController>();
                Debug.Assert(_ballController != null, "GameManager could not find BallController component");
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
        Lose,
        Pause
    }
}