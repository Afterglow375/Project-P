using System;
using Managers;
using UnityEngine;

namespace Gameplay
{
    public class PegController : MonoBehaviour
    {
        public int points;

        public static event Action<int> PegHitEvent;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                gameObject.SetActive(false);
                PegHitEvent?.Invoke(points);
            }
        }
    }
}
