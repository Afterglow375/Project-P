using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.CombatHUD
{
    public static class CombatHUDHelper
    {
        private static readonly Color _flashingTurnColor = new Color(0.1f, .25f, 1f);

        // flashes text color using lerp with sine wave as time
        public static IEnumerator AnimateTurnText(TextMeshProUGUI textMesh)
        {
            float timeAnimating = 0f;
            float shiftPhase = Mathf.PI * 1.5f;
            while (true)
            {
                float speed = 4f;
                float t = (Mathf.Sin(Mathf.PI * timeAnimating * speed + shiftPhase) + 1) / 2.0f;
                timeAnimating += Time.deltaTime;
                textMesh.color = Color.Lerp(Color.clear, _flashingTurnColor, t);
                yield return null;
            }
        }

        public static IEnumerator AnimateDamageTaken(TextMeshProUGUI textMesh)
        {
            Color origColor = textMesh.color;
            float timeAnimating = 0f;
            float fadeDuration = .5f;
            textMesh.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            while (timeAnimating < fadeDuration)
            {
                timeAnimating += Time.deltaTime;
                textMesh.color = Color.Lerp(Color.red, origColor, timeAnimating / fadeDuration);
                yield return null;
            }
        }
    }
}
