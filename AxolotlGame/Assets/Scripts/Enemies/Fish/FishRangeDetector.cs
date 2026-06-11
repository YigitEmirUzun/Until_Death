using UnityEngine;

public class FishRangeDetector : MonoBehaviour
{
    public bool onRange;
    public bool onWall;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onRange = true;
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            onWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onRange = false;
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            onWall = false;
        }
    }
}
