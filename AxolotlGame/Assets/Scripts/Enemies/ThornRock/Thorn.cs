using Unity.Cinemachine;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    public GameObject player;
    public Vector2 targetLocation;
    public float speed;
    float angle;
    public int damage;

    private void Awake()
    {
        player = GameObject.Find("Axolotl");
        targetLocation = (player.transform.position - transform.position).normalized;
        angle = Mathf.Atan2(targetLocation.y, targetLocation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, 3.8f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(targetLocation.x, targetLocation.y + 0.1f, 0) * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponentInParent<IDamageable>();
        if (damageable != null && collision.gameObject.CompareTag("Player"))
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }

}
