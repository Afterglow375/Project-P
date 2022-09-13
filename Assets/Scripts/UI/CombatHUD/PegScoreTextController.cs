using Managers;
using TMPro;
using UnityEngine;

namespace UI.CombatHUD
{
    public class PegScoreTextController : MonoBehaviour
    {
        private TextMeshProUGUI _textMesh;
        private Animator _animator;
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
            _animator = GetComponent<Animator>();
            _textMesh = GetComponent<TextMeshProUGUI>();
            Debug.Assert(_animator != null);
            Debug.Assert(_textMesh != null);
            UpdatePegScoreText(0);
        }

        private void OnPegBonusEvent(int pegScore)
        {
            _animator.SetTrigger(_onPegBonus);
            UpdatePegScoreText(pegScore);
        }

        private void UpdatePegScoreText(int pegScore)
        {
            _textMesh.text = $"Peg score: {pegScore}";
        }
    }
}
