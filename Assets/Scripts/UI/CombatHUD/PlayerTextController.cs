﻿using Managers;
using TMPro;
using UnityEngine;

namespace UI.CombatHUD
{
    public class PlayerTextController : MonoBehaviour
    {
        [SerializeField] private GameObject _playerDamageAnimation;
        private TextMeshProUGUI _playerHealthText;
        private TextMeshProUGUI _playerHealthChangeText;
        private Animator _playerHealthChangeAnimator;
        private int _playerMaxHp;
        private Coroutine _textAnimationCoroutine;
        private Color _origTextColor;
        private readonly int _playerHealthChange = Animator.StringToHash("PlayerDamageTaken");
        private readonly int _noPlayerHealthChange = Animator.StringToHash("NoPlayerDamageTaken");

        private void Awake()
        {
            CombatManager.PlayerHealthChangeEvent += OnPlayerHealthChange;
            CombatManager.PlayerTurnStartEvent += OnPlayerTurnStart;
            CombatManager.PlayerTurnEndEvent += OnPlayerTurnEnd;
        }

        private void OnDestroy()
        {
            CombatManager.PlayerHealthChangeEvent -= OnPlayerHealthChange;
            CombatManager.PlayerTurnStartEvent -= OnPlayerTurnStart;
            CombatManager.PlayerTurnEndEvent -= OnPlayerTurnEnd;
        }

        void Start()
        {
            _playerHealthText = GetComponent<TextMeshProUGUI>();
            _playerHealthChangeText = _playerDamageAnimation.GetComponent<TextMeshProUGUI>();
            _playerHealthChangeAnimator = _playerDamageAnimation.GetComponent<Animator>();
            Debug.Assert(_playerHealthText != null);
            Debug.Assert(_playerHealthChangeText != null);
            Debug.Assert(_playerHealthChangeAnimator != null);
            _playerMaxHp = CombatManager.Instance.GetMaxPlayerHp();
            _origTextColor = _playerHealthText.color;
            UpdatePlayerHealthText(_playerMaxHp);
        }
        
        private void UpdatePlayerHealthText(int hp)
        {
            _playerHealthText.text = $"{hp}/{_playerMaxHp}";
        }

        private void UpdatePlayerHealthAnimationText(int hpChange)
        {
            _playerHealthChangeText.text = $"-{hpChange}";
        }

        private void OnPlayerHealthChange(int hp, int damageTaken)
        {
            UpdatePlayerHealthText(hp);
            UpdatePlayerHealthAnimationText(damageTaken);
            int trigger = damageTaken == 0 ? _noPlayerHealthChange : _playerHealthChange;
            _playerHealthChangeAnimator.SetTrigger(trigger);
            StartCoroutine(CombatHUDHelper.AnimateDamageTaken(_playerHealthText, damageTaken));
        }
        
        private void OnPlayerTurnStart()
        {
            _textAnimationCoroutine = StartCoroutine(CombatHUDHelper.AnimateTurnText(_playerHealthText));
        }

        private void OnPlayerTurnEnd()
        {
            StopCoroutine(_textAnimationCoroutine);
            _playerHealthText.color = _origTextColor;
        }
    }
}