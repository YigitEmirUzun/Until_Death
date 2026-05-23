using UnityEngine;

public class BossRangeDetector : MonoBehaviour
{
    public GameObject player;
    public bool playerInRange = false;

    private void Start()
    {
        player = GameObject.Find("Axolotl");
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
