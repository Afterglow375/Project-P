using System;
using Gameplay;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CombatHUDController : MonoBehaviour
    {
        private TextMeshProUGUI _playerHealth;
        private TextMeshProUGUI _pegScore;
        private TextMeshProUGUI _enemyHealth;
        private int _playerMaxHp;
        private int _enemyMaxHp;

        private void Awake()
        {
            CombatManager.PegScoreUpdateEvent += UpdatePegScoreText;
            CombatManager.PegBonusEvent += OnPegBonusEvent;
            CombatManager.PlayerHealthChangeEvent += OnPlayerHealthChange;
            CombatManager.EnemyHealthChangeEvent += OnEnemyHealthChange;
            CombatManager.PlayerTurnStartEvent += OnPlayerTurnStart;
            CombatManager.PlayerTurnEndEvent += OnPlayerTurnEnd;
            CombatManager.EnemyTurnStartEvent += OnEnemyTurnStart;
            CombatManager.EnemyTurnEndEvent += OnEnemyTurnEnd;
        }

        private void OnDestroy()
        {
            CombatManager.PegScoreUpdateEvent -= UpdatePegScoreText;
            CombatManager.PegBonusEvent -= OnPegBonusEvent;
            CombatManager.PlayerHealthChangeEvent -= OnPlayerHealthChange;
            CombatManager.EnemyHealthChangeEvent -= OnEnemyHealthChange;
            CombatManager.PlayerTurnStartEvent -= OnPlayerTurnStart;
            CombatManager.PlayerTurnEndEvent -= OnPlayerTurnEnd;
            CombatManager.EnemyTurnStartEvent -= OnEnemyTurnStart;
            CombatManager.EnemyTurnEndEvent -= OnEnemyTurnEnd;
        }

        void Start()
        {
            _playerHealth = transform.Find("PlayerTextParent/PlayerText").GetComponent<TextMeshProUGUI>();
            _pegScore = transform.Find("PegScoreTextParent/PegScoreText").GetComponent<TextMeshProUGUI>();
            _enemyHealth = transform.Find("EnemyTextParent/EnemyText").GetComponent<TextMeshProUGUI>();
            _playerMaxHp = CombatManager.Instance.GetMaxPlayerHp();
            _enemyMaxHp = CombatManager.Instance.GetMaxEnemyHp();
            UpdatePlayerHealthText(_playerMaxHp);
            UpdateEnemyHealthText(_enemyMaxHp);
            UpdatePegScoreText();
        }

        private void UpdatePegScoreText(int pegScore = 0)
        {
            _pegScore.text = $"Peg score: {pegScore}";
        }
        
        private void OnPegBonusEvent(int pegScore)
        {
            // TODO: add some crazy text effect when you get peg bonus for funsies
            UpdatePegScoreText(pegScore);
        }

        private void UpdatePlayerHealthText(int hp)
        {
            _playerHealth.text = $"Player health: {hp}/{_playerMaxHp}";
        }

        private void UpdateEnemyHealthText(int hp)
        {
            _enemyHealth.text = $"Enemy health: {hp}/{_enemyMaxHp}";
        }

        private void OnPlayerHealthChange(int hp)
        {
            UpdatePlayerHealthText(hp);
        }
        
        private void OnEnemyHealthChange(int hp)
        {
            UpdateEnemyHealthText(hp);
        }
        
        private void OnPlayerTurnStart()
        {
            
        }

        private void OnPlayerTurnEnd()
        {
            
        }

        private void OnEnemyTurnStart()
        {
            
        }

        private void OnEnemyTurnEnd()
        {
            
        }
    }
}
