using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public bool onWall = false;
    public Vector3 WallPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onWall = true;
            WallPosition = collision.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onWall = false;
            WallPosition = Vector3.zero;
        }
    }
}
