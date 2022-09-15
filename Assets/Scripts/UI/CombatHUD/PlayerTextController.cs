using Managers;
using TMPro;
using UnityEngine;

namespace UI.CombatHUD
{
    public class PlayerTextController : MonoBehaviour
    {
        private TextMeshProUGUI _playerHealthText;
        private TextMeshProUGUI _playerHealthChangeText;
        private Animator _playerHealthChangeAnimator;
        private int _playerMaxHp;
        private Coroutine _textAnimationCoroutine;
        private Color _origTextColor;
        private readonly int _playerHealthChange = Animator.StringToHash("OnPlayerDamageTaken");

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
            _playerHealthText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _playerHealthChangeText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            _playerHealthChangeAnimator = transform.GetChild(1).GetComponent<Animator>();
            Debug.Assert(_playerHealthText != null);
            Debug.Assert(_playerHealthChangeText != null);
            Debug.Assert(_playerHealthChangeAnimator != null);
            _playerMaxHp = CombatManager.Instance.GetMaxPlayerHp();
            _origTextColor = _playerHealthText.color;
            UpdatePlayerHealthText(_playerMaxHp);
        }
        
        private void UpdatePlayerHealthText(int hp)
        {
            _playerHealthText.text = $"Player health: {hp}/{_playerMaxHp}";
        }

        private void UpdatePlayerHealthAnimationText(int hpChange)
        {
            _playerHealthChangeText.text = $"-{hpChange}";
        }

        private void OnPlayerHealthChange(int hp, int damageTaken)
        {
            UpdatePlayerHealthText(hp);
            UpdatePlayerHealthAnimationText(damageTaken);
            _playerHealthChangeAnimator.SetTrigger(_playerHealthChange);
            StartCoroutine(CombatHUDHelper.AnimateDamageTaken(_playerHealthText, damageTaken));
        }
        
        private void OnPlayerTurnStart()
        {
            _textAnimationCoroutine = StartCoroutine(CombatHUDHelper.AnimateTurnText(_playerHealthText));
            _playerHealthText.fontSize += 10;
        }

        private void OnPlayerTurnEnd()
        {
            StopCoroutine(_textAnimationCoroutine);
            _playerHealthText.color = _origTextColor;
            _playerHealthText.fontSize -= 10;
        }
    }
}