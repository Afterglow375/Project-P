using Managers;
using System;
using UnityEngine;

public abstract class APComponent : MonoBehaviour
{
    [SerializeField] protected int _points;
    public static event Action<int, string> HitEvent;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            this.ComponentHit();
        }
    }

    public virtual void ComponentHit()
    {
        CombatManager.Instance.SpawnDamageNumber(_points, transform);
        gameObject.SetActive(false);
        InvokeHitEvent();
    }

    public void InvokeHitEvent()
    {
        HitEvent?.Invoke(_points, this.GetType().Name);
    }
}