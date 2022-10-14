using Gameplay.Balls;
using TMPro;
using UnityEngine;

namespace UI
{
    public class BallTimerText : MonoBehaviour
    {
        private TextMeshProUGUI _ballTimerText;
        private float _ballDuration;

        private void Awake()
        {
            Ball.BallTimerChange += UpdateBallTimer;
            LauncherController.BallSwitched += BallSwitched;
        }

        private void OnDestroy()
        {
            Ball.BallTimerChange -= UpdateBallTimer;
            LauncherController.BallSwitched -= BallSwitched;
        }
        
        private void Start()
        {
            _ballTimerText = GetComponent<TextMeshProUGUI>();
            FormatTimer(_ballDuration);
        }

        private void UpdateBallTimer(float duration)
        {
            if (duration > 0)
            {
                _ballTimerText.SetText(FormatTimer(duration));
            }
            else
            {
                _ballTimerText.SetText(FormatTimer(_ballDuration));
            }
        }

        private string FormatTimer(float f)
        {
            return f.ToString("0.0");
        }

        private void BallSwitched(float duration)
        {
            _ballDuration = duration;
            _ballTimerText.SetText(FormatTimer(_ballDuration));
        }
    }
}