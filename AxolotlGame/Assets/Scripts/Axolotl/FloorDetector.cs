using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    public bool OnFloor = false;
    public Rigidbody2D PlayerRb;
    void Start()
    {
        PlayerRb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            OnFloor = true;
            PlayerRb.linearVelocityX = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            OnFloor = false;
        }
    }
}
