using Managers;
using TMPro;
using UnityEngine;

namespace UI.CombatHUD
{
    public class PegScoreTextController : MonoBehaviour
    {
        private TextMeshProUGUI _pegScoreText;
        private TextMeshProUGUI _pegBonusText;
        private Animator _pegScoreAnimator;
        private Animator _pegBonusAnimator;
        private readonly int _onPegBonus = Animator.StringToHash("OnPegBonus");

        private void Awake()
        {
            CombatManager.PegBonusEvent += OnPegBonusEvent;
            CombatManager.PegScoreUpdateEvent += UpdatePegScoreText;
        }

        private void OnDestroy()
        {
            CombatManager.PegBonusEvent -= OnPegBonusEvent;
            CombatManager.PegScoreUpdateEvent -= UpdatePegScoreText;
        }

        void Start()
        {
            _pegScoreText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _pegScoreAnimator = transform.GetChild(0).GetComponent<Animator>();
            _pegBonusText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            _pegBonusAnimator = transform.GetChild(1).GetComponent<Animator>();
            Debug.Assert(_pegScoreText != null);
            Debug.Assert(_pegScoreAnimator != null);
            Debug.Assert(_pegBonusText != null);
            Debug.Assert(_pegBonusAnimator != null);
            UpdatePegScoreText(0);
        }
        
        private void UpdatePegScoreText(int pegScore)
        {
            _pegScoreText.text = $"Peg score: {pegScore}";
        }
        
        private void UpdatePegBonusText(int pegBonus)
        {
            _pegBonusText.text = $"+{pegBonus}";
        }

        private void OnPegBonusEvent(int pegBonus)
        {
            _pegScoreAnimator.SetTrigger(_onPegBonus);
            UpdatePegBonusText(pegBonus);
            _pegBonusAnimator.SetTrigger(_onPegBonus);
        }
    }
}
