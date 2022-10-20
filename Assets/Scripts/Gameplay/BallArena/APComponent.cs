using System;
using Managers;
using UnityEngine;

namespace Gameplay.BallArena
{
    public abstract class APComponent : MonoBehaviour
    {
        [SerializeField] protected int _points;
        public static event Action<int, string> HitEvent;

        protected bool IsBallCollided(GameObject other)
        {
            return other.gameObject.CompareTag("Ball");
        }

        public virtual void ComponentHit()
        {
            CombatManager.Instance.SpawnDamageNumber(_points, transform);
            HitEvent?.Invoke(_points, GetType().Name);
        }

        protected virtual void ComponentHit(Collision2D collision)
        {
            ComponentHit();
        }
    }
}
