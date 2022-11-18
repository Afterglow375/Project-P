using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Balls
{
    public class PowerBarController : MonoBehaviour
    {
        [SerializeField] private Image _powerBar;
        [SerializeField] private GameObject _powerBarCanvas;
        [SerializeField] private float powerBarChangeIncrement = 0.05f;
        private float _timeHeld;

        void Update()
        {
            if (GameManager.Instance.state == GameState.ReadyToShoot)
            {
                if (!_powerBarCanvas.activeSelf) _powerBarCanvas.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W))
                {
                    _timeHeld = 0;
                }
                
                if (Input.GetKey(KeyCode.Q))
                {
                    UpdatePowerBar(-powerBarChangeIncrement);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    UpdatePowerBar(powerBarChangeIncrement);
                }

            }
            else if (_powerBarCanvas.activeSelf)
            {
                _powerBarCanvas.SetActive(false);
            }

        }

        private void UpdatePowerBar(float powerBarIncrement)
        {
            _timeHeld += Time.deltaTime;
            _powerBar.fillAmount += powerBarIncrement * (_timeHeld / 4);
        }

        public float GetPowerModifier()
        {
            return _powerBar.fillAmount;
        }
    }
}
