using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    GameObject Player;
    Rigidbody2D PlayerRb;
    void Awake()
    {
        Player = GameObject.Find("Axolotl");
        PlayerRb = Player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerRb != null)
        {
            if (collision.gameObject.CompareTag("JumpFlower"))
            {
                if(Player.transform.position.y > collision.gameObject.transform.position.y)
                {
                    Vector3 distance = Player.transform.position - collision.gameObject.transform.position;
                    if(distance.y < 3 ){ distance = new Vector3(distance.x, 3f, 0f); }
                    PlayerRb.linearVelocity = new Vector2(0f,0f);
                    PlayerRb.AddForce(new Vector3(0, distance.y * 10f, 0f),ForceMode2D.Impulse);
                }
            }

        }
    }
}
