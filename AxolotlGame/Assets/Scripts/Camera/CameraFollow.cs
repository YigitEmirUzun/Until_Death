using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    void Awake()
    {
        Player = GameObject.Find("Axolotl");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,Player.transform.position.y + 4,transform.position.z);
    }
}
