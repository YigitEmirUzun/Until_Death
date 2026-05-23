using System;
using System.Threading;
using UnityEngine;

public class DragonPlatformTurn : MonoBehaviour
{
    float touchTimer = 0.3f;
    float turnbackTimer = 3;
    bool playerTouched = false;
    float zRotation = 180;
    public int damage = 20;

    private void Update()
    {
        if (playerTouched)
        {
            touchTimer -= Time.deltaTime;
        }

        if(touchTimer <= 0)
        {
            zRotation = Mathf.MoveTowards(zRotation, 0, 10f);
            playerTouched = false;
            turnbackTimer = 3;
        }

        if(zRotation <= 0)
        {
            turnbackTimer -= Time.deltaTime;
            touchTimer = 0.5f;

        }

        if(turnbackTimer <= 0)
        {
            zRotation = Mathf.MoveTowards(zRotation, 180, 5f);
            
        }
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FloorDetector"))
        {
            playerTouched = true;
            Debug.Log("Touched");
        }
    }




}
