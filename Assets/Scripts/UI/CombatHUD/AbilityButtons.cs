using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CombatHUD
{
    public class AbilityButtons : MonoBehaviour
    {
        private Button[] _buttons;
        
        private void Awake()
        {
            CombatManager.PlayerTurnStartEvent += ActivateButtons;
            CombatManager.PlayerTurnEndEvent += DisableButtons;
        }
        
        private void OnDestroy()
        {
            CombatManager.PlayerTurnStartEvent -= ActivateButtons;
            CombatManager.PlayerTurnEndEvent -= DisableButtons;
        }
        
        private void Start()
        {
            _buttons = transform.GetComponentsInChildren<Button>();
        }

        public void ActivateButtons()
        {
            int points = CombatManager.Instance.GetAbilityPoints();
            foreach (var button in _buttons)
            {
                string buttonText = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                if (int.Parse(buttonText) <= points)
                {
                    button.interactable = true;
                }
            }
        }
        
        public void DisableButtons()
        {
            foreach (var button in _buttons)
            {
                button.interactable = false;
            }
        }

        public void ButtonClicked(int abilityPoints)
        {
            DisableButtons();
            CombatManager.Instance.DoPlayerAttack(abilityPoints);
        }
    }
}