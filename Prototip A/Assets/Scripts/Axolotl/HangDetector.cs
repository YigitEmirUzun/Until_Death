using UnityEngine;

public class HangDetector : MonoBehaviour
{
    public bool canHang = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HangFlower"))
        {
            canHang = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HangFlower"))
        {
            canHang = false;
            Time.timeScale = 1f;
        }
    }
}
