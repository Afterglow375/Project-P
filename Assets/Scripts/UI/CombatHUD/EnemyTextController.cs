using Managers;
using TMPro;
using UnityEngine;

namespace UI.CombatHUD
{
    public class EnemyTextController : MonoBehaviour
    {
        private TextMeshProUGUI _textMesh;
        private int _enemyMaxHp;
        private Coroutine _textAnimationCoroutine;
        private static Color _origTextColor;
        
        private void Awake()
        {
            CombatManager.EnemyHealthChangeEvent += OnEnemyHealthChange;
            CombatManager.EnemyTurnStartEvent += OnEnemyTurnStart;
            CombatManager.EnemyTurnEndEvent += OnEnemyTurnEnd;
        }

        private void OnDestroy()
        {
            CombatManager.EnemyHealthChangeEvent -= OnEnemyHealthChange;
            CombatManager.EnemyTurnStartEvent -= OnEnemyTurnStart;
            CombatManager.EnemyTurnEndEvent -= OnEnemyTurnEnd;
        }

        void Start()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
            Debug.Assert(_textMesh != null);
            _enemyMaxHp = CombatManager.Instance.GetMaxEnemyHp();
            _origTextColor = _textMesh.color;
            UpdateEnemyHealthText(_enemyMaxHp);
        }
        
        private void UpdateEnemyHealthText(int hp)
        {
            _textMesh.text = $"Enemy health: {hp}/{_enemyMaxHp}";
        }

        private void OnEnemyHealthChange(int hp)
        {
            UpdateEnemyHealthText(hp);
            StartCoroutine(CombatHUDHelper.AnimateDamageTaken(_textMesh));
        }
        
        private void OnEnemyTurnStart()
        {
            _textAnimationCoroutine = StartCoroutine(CombatHUDHelper.AnimateTurnText(_textMesh));
            _textMesh.fontSize += 10;
        }

        private void OnEnemyTurnEnd()
        {
            StopCoroutine(_textAnimationCoroutine);
            _textMesh.color = _origTextColor;
            _textMesh.fontSize -= 10;
        }
    }
}