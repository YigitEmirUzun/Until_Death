using UnityEngine;

public class SnailDetector : MonoBehaviour , IDamageable
{
    public Rigidbody2D rb;
    public float speed;
    Animator anim;
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("attacking", true);
            if(collision.transform.position.x > transform.position.x)
            {
                rb.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
                Debug.Log("Tetiklendi.");
            }
            else
            {
                rb.AddForce(Vector2.left * speed, ForceMode2D.Impulse);
                Debug.Log("Tetiklendi.");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Destroy(transform.parent.gameObject);
        Debug.Log("Hitted");
    }
}
