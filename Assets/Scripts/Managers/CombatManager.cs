using System;
using System.Collections;
using Gameplay.BallArena;
using TMPro;
using UI.CombatHUD;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Managers
{
    /// <summary>
    /// The combat manager is a singleton which stores data on the gameplay taking place (player health, enemy health, peg score, etc).
    /// </summary>
    public class CombatManager : Singleton<CombatManager>
    {
        [SerializeField] private int _comboBonus = 1;
        [SerializeField] private int _playerMaxHp;
        [SerializeField] private int _enemyMaxHp;
        [SerializeField] private int _minEnemyDamage = 0;
        [SerializeField] private int _maxEnemyDamage = 30;

        private Coroutine _playerTurnAnimation;
        private AbilityButtons _abilityButtons;

        private int _componentsHit;
        private int _abilityPoints;
        private int _currPlayerHp;
        private int _currEnemyHp;

        private GameObject _damageNumberPrefab;

        // params: total player ability points
        public static event Action<int> AbilityPointsUpdateEvent;
        // params: combo bonus
        public static event Action<int> ComboBonusEvent;
        // params: player hp after taking dmg, enemy damage
        public static event Action<int, int> PlayerHealthChangeEvent;
        // params: enemy hp after taking dmg, player damage
        public static event Action<int, int> EnemyHealthChangeEvent;
        public static event Action PlayerTurnStartEvent;
        public static event Action PlayerTurnEndEvent;
        public static event Action EnemyTurnStartEvent;
        public static event Action EnemyTurnEndEvent;
        public static event Action LevelVictoryEvent;
        public static event Action GameVictoryEvent;
        public static event Action LevelFailedEvent;
        public static event Action TargetHitEvent;

        private void OnDestroy()
        {
            PegController.PegHitEvent -= ComponentHitByBall;
            ForceComponentController.ComponentHitEvent -= ComponentHitByBall;
            DiamondController.DiamondHitEvent -= ComponentHitByBall;
        }

        private void Start()
        {
            PegController.PegHitEvent += ComponentHitByBall;
            ForceComponentController.ComponentHitEvent += ComponentHitByBall;
            DiamondController.DiamondHitEvent += ComponentHitByBall;
            
            _currPlayerHp = _playerMaxHp;
            _currEnemyHp = _enemyMaxHp;
            _abilityButtons = GameObject.Find("AbilityButtons").GetComponent<AbilityButtons>();

            // make enemy damage a multiple of 5
            _minEnemyDamage /= 5;
            _maxEnemyDamage /= 5;
            
            _damageNumberPrefab = Resources.Load("Prefabs/DamageNumber") as GameObject;
        }

        private void ComponentHitByBall(int score)
        {
            _abilityPoints += score;
            _componentsHit++;
            AbilityPointsUpdateEvent?.Invoke(_abilityPoints);
            if (_componentsHit % 5 == 0)
            {
                _abilityPoints += _comboBonus;
                ComboBonusEvent?.Invoke(_comboBonus);
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
            TargetHitEvent?.Invoke();
            yield return new WaitForSeconds(1);
            if (_currEnemyHp <= 0)
            {
                if (GameManager.Instance.IsLastLevel())
                {
                    GameManager.Instance.UpdateGameState(GameState.GameVictory);
                    GameVictoryEvent?.Invoke();
                }
                else
                {
                    GameManager.Instance.UpdateGameState(GameState.LevelVictory);
                    LevelVictoryEvent?.Invoke();
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
            TargetHitEvent?.Invoke();
            yield return new WaitForSeconds(1);
            if (_currPlayerHp <= 0)
            {
                GameManager.Instance.UpdateGameState(GameState.LevelFailed);
                LevelFailedEvent?.Invoke();
                yield break;
            }
            EnemyTurnEndEvent?.Invoke();
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
