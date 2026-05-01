using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    public bool followX = true;
    void Awake()
    {
        Player = GameObject.Find("Axolotl");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,Player.transform.position.y + 4,transform.position.z);

        if (followX)
        {
            transform.position = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
