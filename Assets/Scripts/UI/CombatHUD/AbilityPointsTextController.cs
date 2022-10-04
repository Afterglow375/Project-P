using Managers;
using TMPro;
using UnityEngine;

namespace UI.CombatHUD
{
    public class AbilityPointsTextController : MonoBehaviour
    {
        private TextMeshProUGUI _abilityPointsText;
        private TextMeshProUGUI _comboBonusText;
        private Animator _abilityPointsAnimator;
        private Animator _comboBonusAnimator;
        private readonly int _onComboBonus = Animator.StringToHash("OnComboBonus");

        private void Awake()
        {
            CombatManager.ComboBonusEvent += OnComboBonusEvent;
            CombatManager.AbilityPointsUpdateEvent += UpdateAbilityPointsText;
        }

        private void OnDestroy()
        {
            CombatManager.ComboBonusEvent -= OnComboBonusEvent;
            CombatManager.AbilityPointsUpdateEvent -= UpdateAbilityPointsText;
        }

        void Start()
        {
            _abilityPointsText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _abilityPointsAnimator = transform.GetChild(0).GetComponent<Animator>();
            _comboBonusText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            _comboBonusAnimator = transform.GetChild(1).GetComponent<Animator>();
            Debug.Assert(_abilityPointsText != null);
            Debug.Assert(_abilityPointsAnimator != null);
            Debug.Assert(_comboBonusText != null);
            Debug.Assert(_comboBonusAnimator != null);
            UpdateAbilityPointsText(0);
        }
        
        private void UpdateAbilityPointsText(int abilityPoints)
        {
            _abilityPointsText.text = $"AP: {abilityPoints}";
        }
        
        private void UpdateComboBonusText(int comboBonus)
        {
            _comboBonusText.text = $"+{comboBonus}";
        }

        private void OnComboBonusEvent(int comboBonus)
        {
            _abilityPointsAnimator.SetTrigger(_onComboBonus);
            UpdateComboBonusText(comboBonus);
            _comboBonusAnimator.SetTrigger(_onComboBonus);
        }
    }
}
