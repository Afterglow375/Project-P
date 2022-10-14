using Managers;
using TMPro;
using UnityEngine;

namespace UI.CombatHUD
{
    public class EnemyTextController : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyDamageAnimation;
        private TextMeshProUGUI _enemyHealthText;
        private TextMeshProUGUI _enemyHealthChangeText;
        private Animator _enemyHealthChangeAnimator;
        private int _enemyMaxHp;
        private Coroutine _textAnimationCoroutine;
        private Color _origTextColor;
        private readonly int _enemyHealthChange = Animator.StringToHash("EnemyDamageTaken");
        private readonly int _noEnemyHealthChange = Animator.StringToHash("NoEnemyDamageTaken");

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
            _enemyHealthText = GetComponent<TextMeshProUGUI>();
            _enemyHealthChangeText = _enemyDamageAnimation.GetComponent<TextMeshProUGUI>();
            _enemyHealthChangeAnimator = _enemyDamageAnimation.GetComponent<Animator>();
            Debug.Assert(_enemyHealthText != null);
            Debug.Assert(_enemyHealthChangeText != null);
            Debug.Assert(_enemyHealthChangeAnimator != null);
            _enemyMaxHp = CombatManager.Instance.GetMaxEnemyHp();
            _origTextColor = _enemyHealthText.color;
            UpdateEnemyHealthText(_enemyMaxHp);
        }
        
        private void UpdateEnemyHealthText(int hp)
        {
            _enemyHealthText.text = $"{hp}/{_enemyMaxHp}";
        }

        private void UpdateEnemyHealthAnimationText(int hpChange)
        {
            _enemyHealthChangeText.text = $"-{hpChange}";
        }

        private void OnEnemyHealthChange(int hp, int damageTaken)
        {
            UpdateEnemyHealthText(hp);
            UpdateEnemyHealthAnimationText(damageTaken);
            int trigger = damageTaken == 0 ? _noEnemyHealthChange : _enemyHealthChange;
            _enemyHealthChangeAnimator.SetTrigger(trigger);
            StartCoroutine(CombatHUDHelper.AnimateDamageTaken(_enemyHealthText, damageTaken));
        }
        
        private void OnEnemyTurnStart()
        {
            _textAnimationCoroutine = StartCoroutine(CombatHUDHelper.AnimateTurnText(_enemyHealthText));
        }

        private void OnEnemyTurnEnd()
        {
            StopCoroutine(_textAnimationCoroutine);
            _enemyHealthText.color = _origTextColor;
        }
    }
}