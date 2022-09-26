using Managers;
using TMPro;
using UnityEngine;

namespace UI.CombatHUD
{
    public class AbilityPointsTextController : MonoBehaviour
    {
        private TextMeshProUGUI _abilityPointsText;
        private TextMeshProUGUI _pegBonusText;
        private Animator _abilityPointsAnimator;
        private Animator _pegBonusAnimator;
        private readonly int _onPegBonus = Animator.StringToHash("OnPegBonus");

        private void Awake()
        {
            CombatManager.PegBonusEvent += OnPegBonusEvent;
            CombatManager.AbilityPointsUpdateEvent += UpdateAbilityPointsText;
        }

        private void OnDestroy()
        {
            CombatManager.PegBonusEvent -= OnPegBonusEvent;
            CombatManager.AbilityPointsUpdateEvent -= UpdateAbilityPointsText;
        }

        void Start()
        {
            _abilityPointsText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _abilityPointsAnimator = transform.GetChild(0).GetComponent<Animator>();
            _pegBonusText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            _pegBonusAnimator = transform.GetChild(1).GetComponent<Animator>();
            Debug.Assert(_abilityPointsText != null);
            Debug.Assert(_abilityPointsAnimator != null);
            Debug.Assert(_pegBonusText != null);
            Debug.Assert(_pegBonusAnimator != null);
            UpdateAbilityPointsText(0);
        }
        
        private void UpdateAbilityPointsText(int abilityPoints)
        {
            _abilityPointsText.text = $"AP: {abilityPoints}";
        }
        
        private void UpdatePegBonusText(int pegBonus)
        {
            _pegBonusText.text = $"+{pegBonus}";
        }

        private void OnPegBonusEvent(int pegBonus)
        {
            _abilityPointsAnimator.SetTrigger(_onPegBonus);
            UpdatePegBonusText(pegBonus);
            _pegBonusAnimator.SetTrigger(_onPegBonus);
        }
    }
}
