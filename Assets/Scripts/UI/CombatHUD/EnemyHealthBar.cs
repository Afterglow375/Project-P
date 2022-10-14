using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CombatHUD
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Image _healthBarSprite;
        private int _maxHealth;
        
        private void Awake()
        {
            CombatManager.EnemyHealthChangeEvent += UpdateHealthBar;
        }

        private void Start()
        {
            _maxHealth = CombatManager.Instance.GetMaxEnemyHp();
        }

        private void OnDestroy()
        {
            CombatManager.EnemyHealthChangeEvent -= UpdateHealthBar;
        }

        public void UpdateHealthBar(int currentHealth, int damage)
        {
            _healthBarSprite.fillAmount = (float) currentHealth / _maxHealth;
        }
    }
}