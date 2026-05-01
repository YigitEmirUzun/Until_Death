using UnityEngine;

public class ThornRock : MonoBehaviour
{
    public GameObject thorn;
    float attackTimer = 2;
    bool inRange;

    GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Axolotl");
    }

    private void Update()
    {
        if (inRange) 
        {
            attackTimer += Time.deltaTime;

            if (attackTimer > 3)
            {
                attackTimer = 0;
                Instantiate(thorn, transform.position, Quaternion.identity);
            }
        }

        if ((transform.position.y + 20 > player.transform.position.y) && (transform.position.y - 20 < player.transform.position.y))
        {
            inRange = true;
        }
        else { inRange = false; }

    }
}
