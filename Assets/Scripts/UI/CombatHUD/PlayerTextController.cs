using Managers;
using TMPro;
using UnityEngine;

namespace UI.CombatHUD
{
    public class PlayerTextController : MonoBehaviour
    {
        private TextMeshProUGUI _textMesh;
        private int _playerMaxHp;
        private Coroutine _textAnimationCoroutine;
        private static Color _origTextColor;
        
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
            _textMesh = GetComponent<TextMeshProUGUI>();
            Debug.Assert(_textMesh != null);
            _playerMaxHp = CombatManager.Instance.GetMaxPlayerHp();
            _origTextColor = _textMesh.color;
            UpdatePlayerHealthText(_playerMaxHp);
        }
        
        private void UpdatePlayerHealthText(int hp)
        {
            _textMesh.text = $"Player health: {hp}/{_playerMaxHp}";
        }

        private void OnPlayerHealthChange(int hp)
        {
            UpdatePlayerHealthText(hp);
        }
        
        private void OnPlayerTurnStart()
        {
            _textAnimationCoroutine = StartCoroutine(CombatHUDHelper.AnimateTurnText(_textMesh));
            _textMesh.fontSize += 10;
        }

        private void OnPlayerTurnEnd()
        {
            StopCoroutine(_textAnimationCoroutine);
            _textMesh.color = _origTextColor;
            _textMesh.fontSize -= 10;
        }
    }
}