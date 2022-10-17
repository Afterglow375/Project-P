using System;
using Managers;
using UnityEngine;

namespace Gameplay.BallArena
{
    public class DiamondController : MonoBehaviour
    {
        [SerializeField] private int _points;
        public static event Action<int> DiamondHitEvent;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Ball"))
            {
                gameObject.SetActive(false);
                CombatManager.Instance.SpawnDamageNumber(_points, transform);
                DiamondHitEvent?.Invoke(_points);
            }
        }
    }
}