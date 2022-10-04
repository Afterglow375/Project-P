using System;
using System.Collections;
using Gameplay;
using TMPro;
using UI;
using UI.CombatHUD;
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

        private Coroutine _playerTurnAnimation;
        private AbilityButtons _abilityButtons;

        private int _pegCount;
        private int _abilityPoints;
        private int _currPlayerHp;
        private int _currEnemyHp;

        private GameObject _damageNumberPrefab;

        public static event Action<int> AbilityPointsUpdateEvent;
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
                return;
            }

            Instance = this;
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
            _abilityButtons = GameObject.Find("AbilityButtons").GetComponent<AbilityButtons>();
            Debug.Assert(_ballController != null, "GameManager could not find BallController component");

            // make enemy damage a multiple of 5
            _minEnemyDamage /= 5;
            _maxEnemyDamage /= 5;
            
            _damageNumberPrefab = Resources.Load("Prefabs/DamageNumber") as GameObject;
        }

        private void PegHitByBall(int score)
        {
            _abilityPoints += score;
            _pegCount++;
            AbilityPointsUpdateEvent?.Invoke(_abilityPoints);
            if (_pegCount % 5 == 0)
            {
                _abilityPoints += _pegBonus;
                PegBonusEvent?.Invoke(_pegBonus);
                AbilityPointsUpdateEvent?.Invoke(_abilityPoints);
            }
        }

        // change state to player's turn and wait for player to click ability
        public void DoCombat()
        {
            GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
            PlayerTurnStartEvent?.Invoke();
            if (_abilityPoints < 10) // make player do 0 dmg if they can't afford an ability
            {
                DoPlayerAttack(0);
            }
            else
            {
                _abilityButtons.ActivateButtons();
            }
        }

        public void DoPlayerAttack(int abilityPoints)
        {
            _abilityPoints -= abilityPoints;
            AbilityPointsUpdateEvent?.Invoke(_abilityPoints);
            StartCoroutine(PlayerAttack(abilityPoints));
        }

        // for now ability points will just be damage
        private IEnumerator PlayerAttack(int abilityPoints)
        {
            yield return new WaitForSeconds(1);
            Debug.Log($"Player damage: {abilityPoints}");
            _currEnemyHp -= abilityPoints;
            EnemyHealthChangeEvent?.Invoke(_currEnemyHp, abilityPoints);
            yield return new WaitForSeconds(1);
            if (_currEnemyHp <= 0)
            {
                GameManager.Instance.UpdateGameState(GameState.LevelVictory);
                LevelVictoryEvent?.Invoke();
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

        public int GetAbilityPoints()
        {
            return _abilityPoints;
        }

        public void SpawnDamageNumber(int damage, Transform parentTransform)
        {
            GameObject damageNumber = Instantiate(_damageNumberPrefab, parentTransform.position, Quaternion.identity);
            damageNumber.GetComponentInChildren<TextMeshPro>().SetText(damage.ToString());
        }
    }
}
