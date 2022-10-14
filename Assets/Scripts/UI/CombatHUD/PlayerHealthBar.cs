using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CombatHUD
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private Image _healthBarSprite;
        private int _maxHealth;
        
        private void Awake()
        {
            CombatManager.PlayerHealthChangeEvent += UpdateHealthBar;
        }

        private void Start()
        {
            _maxHealth = CombatManager.Instance.GetMaxPlayerHp();
        }

        private void OnDestroy()
        {
            CombatManager.PlayerHealthChangeEvent -= UpdateHealthBar;
        }

        public void UpdateHealthBar(int currentHealth, int damage)
        {
            _healthBarSprite.fillAmount = (float) currentHealth / _maxHealth;
        }
    }
}