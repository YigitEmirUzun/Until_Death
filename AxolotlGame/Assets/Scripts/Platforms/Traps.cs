using UnityEngine;

public class Traps : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();
        if (damageable != null && collision.gameObject.CompareTag("Player"))
        {
            damageable.TakeDamage(30);
        }
    }
}
