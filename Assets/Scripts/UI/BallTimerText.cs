using Gameplay;
using Gameplay.Balls;
using TMPro;
using UnityEngine;

namespace UI
{
    public class BallTimerText : MonoBehaviour
    {
        private TextMeshProUGUI _ballTimerText;

        private void Awake()
        {
            BasicBall.BallTimerChange += UpdateBallTimer;
        }

        private void OnDestroy()
        {
            BasicBall.BallTimerChange -= UpdateBallTimer;
        }
        
        private void Start()
        {
            _ballTimerText = GetComponent<TextMeshProUGUI>();
        }

        private void UpdateBallTimer(float duration)
        {
            if (duration > 0)
            {
                _ballTimerText.SetText(duration.ToString("0.0"));
            }
            else
            {
                _ballTimerText.SetText("");
            }
        }
    }
}