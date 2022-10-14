using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Balls
{
    public class PowerBarController : MonoBehaviour
    {
        [SerializeField] private Image _powerBar;
        private GameObject _powerBarCanvas;

        public float shotPowerModifier;
        public float scrollIncrement = 0.05f;

        void Start()
        {
            _powerBarCanvas = GameObject.Find("PowerBarCanvas");
        }

        void Update()
        {
            shotPowerModifier = _powerBar.fillAmount;

            if (GameManager.Instance.state == GameState.ReadyToShoot)
            {
                _powerBarCanvas.SetActive(true);

                if (Input.mouseScrollDelta.y > 0)
                {
                    _powerBar.fillAmount += scrollIncrement;
                }
                else if (Input.mouseScrollDelta.y < 0)
                {
                    _powerBar.fillAmount -= scrollIncrement;
                }
            }
            else
            {
                _powerBarCanvas.SetActive(false);
            }
        }
    }
}
