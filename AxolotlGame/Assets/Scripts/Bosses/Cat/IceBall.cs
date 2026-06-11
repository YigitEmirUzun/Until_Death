using UnityEngine;

public class IceBall : MonoBehaviour
{
    public bool hitted = false;
    Rigidbody2D rb;
    GameObject player;
    PlayerController playerController;

    public int damage = 20;
    void Start()
    {
        player = GameObject.Find("Axolotl");
        rb = GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hitted)
        {
            transform.Translate(new Vector3(0,-15 * Time.deltaTime,0));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            hitted = true;
            if (playerController.IsRight)
            {
                rb.AddForce(new Vector2(50, 0), ForceMode2D.Impulse);
            }
            else { rb.AddForce(new Vector2(-50, 0), ForceMode2D.Impulse); }

        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }

        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}

