using System;
using System.Collections;
using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Utilities;
using Random = UnityEngine.Random;

namespace Managers
{
    /// <summary>
    /// The combat manager is a singleton which stores data on the gameplay taking place (player health, enemy health, peg score, etc).
    /// </summary>
    public class CombatManager : MonoBehaviour
    {
        private static CombatManager _instance;
        public static CombatManager Instance { get; private set; }
        
        [SerializeField] private int _pegBonus = 10;
        [SerializeField] private int _playerMaxHp;
        [SerializeField] private int _enemyMaxHp;
        [SerializeField] private int _minEnemyDamage = 0;
        [SerializeField] private int _maxEnemyDamage = 30;

        private int _pegCount;
        private int _pegScore;
        private int _currPlayerHp;
        private int _currEnemyHp;

        public static event Action<int> PegScoreUpdateEvent;
        public static event Action<int> PegBonusEvent;
        public static event Action<int, int> PlayerHealthChangeEvent;
        public static event Action<int, int> EnemyHealthChangeEvent;
        public static event Action PlayerTurnStartEvent;
        public static event Action PlayerTurnEndEvent;
        public static event Action EnemyTurnStartEvent;
        public static event Action EnemyTurnEndEvent;
        public static event Action LevelVictoryEvent;
        public static event Action GameVictoryEvent;
        public static event Action LevelFailedEvent;
        
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

            PegController.PegHitEvent += PegHitByBall;
        }

        private void OnDestroy()
        {
            PegController.PegHitEvent -= PegHitByBall;
        }

        private void Start()
        {
            _currPlayerHp = _playerMaxHp;
            _currEnemyHp = _enemyMaxHp;
            _ballController = GameObject.FindWithTag("Ball").GetComponent<BallController>();
            Debug.Assert(_ballController != null, "GameManager could not find BallController component");
            
            // make enemy damage a multiple of 5
            _minEnemyDamage /= 5;
            _maxEnemyDamage /= 5;
        }

        private void PegHitByBall(int score)
        {
            _pegScore += score;
            _pegCount++;
            PegScoreUpdateEvent?.Invoke(_pegScore);
            if (_pegCount % 5 == 0)
            {
                _pegScore += _pegBonus;
                PegBonusEvent?.Invoke(_pegBonus);
                PegScoreUpdateEvent?.Invoke(_pegScore);
            }
        }

        public void DoCombat()
        {
            StartCoroutine(PlayerTurn());
        }

        private IEnumerator PlayerTurn()
        {
            GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
            PlayerTurnStartEvent?.Invoke();
            yield return new WaitForSeconds(1);
            Debug.Log($"Pegs hit: {_pegCount}, Peg score (player attack damage): {_pegScore}");
            _currEnemyHp -= _pegScore;
            EnemyHealthChangeEvent?.Invoke(_currEnemyHp, _pegScore);
            yield return new WaitForSeconds(1);
            if (_currEnemyHp <= 0)
            {
                if (SceneManager.GetActiveScene().name != Scenes.Level6)
                {
                    GameManager.Instance.UpdateGameState(GameState.LevelVictory);
                    LevelVictoryEvent?.Invoke();
                }
                else
                {
                    GameManager.Instance.UpdateGameState(GameState.GameVictory);
                    GameVictoryEvent?.Invoke();
                }
                
                yield break;
            }
            PlayerTurnEndEvent?.Invoke();
            StartCoroutine(EnemyTurn());
        }
        
        private IEnumerator EnemyTurn()
        {
            GameManager.Instance.UpdateGameState(GameState.EnemyTurn);
            EnemyTurnStartEvent?.Invoke();
            yield return new WaitForSeconds(1);
            int enemyDamage = Random.Range(_minEnemyDamage, _maxEnemyDamage+1)*5;
            Debug.Log($"Enemy damage: {enemyDamage}");
            _currPlayerHp -= enemyDamage;
            PlayerHealthChangeEvent?.Invoke(_currPlayerHp, enemyDamage);
            yield return new WaitForSeconds(1);
            if (_currPlayerHp <= 0)
            {
                GameManager.Instance.UpdateGameState(GameState.LevelFailed);
                LevelFailedEvent?.Invoke();
                yield break;
            }
            EnemyTurnEndEvent?.Invoke();
            ResetBall();
        }

        private void ResetBall()
        {
            GameManager.Instance.UpdateGameState(GameState.ResettingBall);
            _ballController.ResetPos();
            ResetPegScore();
            GameManager.Instance.UpdateGameState(GameState.ReadyToShoot);
        }

        public int GetMaxPlayerHp()
        {
            return _playerMaxHp;
        }
        
        public int GetMaxEnemyHp()
        {
            return _enemyMaxHp;
        }

        private void ResetPegScore()
        {
            _pegScore = 0;
            _pegCount = 0;
            PegScoreUpdateEvent?.Invoke(_pegScore);
        }
    }
}
