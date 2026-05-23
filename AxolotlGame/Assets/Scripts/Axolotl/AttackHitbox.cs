using System;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 10;
    public static Action OnHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if(damageable != null)
        {
            damageable.TakeDamage(damage);
            if (!collision.gameObject.CompareTag("Player"))
            {
                OnHit?.Invoke();
            }

        }
    }
}
