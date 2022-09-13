using Managers;
using UnityEngine;

namespace UI
{
    public class PegScoreTextController : MonoBehaviour
    {
        private Animator _animator;
        private readonly int _onPegBonus = Animator.StringToHash("OnPegBonus");

        private void Awake()
        {
            CombatManager.PegBonusEvent += OnPegBonusEvent;
        }

        private void OnDestroy()
        {
            CombatManager.PegBonusEvent -= OnPegBonusEvent;
        }

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnPegBonusEvent(int pegScore)
        {
            _animator.SetTrigger(_onPegBonus);
        }
    }
}
