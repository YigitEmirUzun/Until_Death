using UnityEngine;

public class SnailController : MonoBehaviour
{
    public int damage = 20;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();
        if (damageable != null && collision.gameObject.CompareTag("Player"))
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            Destroy(gameObject);
            Debug.Log("vurdun");
        }
    }
}
