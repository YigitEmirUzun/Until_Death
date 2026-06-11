using Spine.Unity;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class KittyMovement : MonoBehaviour, IDamageable
{
    SkeletonAnimation anim;
    Rigidbody2D rb;

    public static Action KittyDied;

    public bool isRight;
    public bool onFloor;
    public int Health = 7;
    public float speed;

    public float jumpTimer;

    public KittyState currentState;
    public KittyState lastState;

    public enum KittyState
    {
        Running,
        Jumping
    }

    private void Awake()
    {
        anim = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>();
        speed = UnityEngine.Random.Range(4f, 8f);
        jumpTimer = UnityEngine.Random.Range(1.6f, 8f);
    }
    void Start()
    {
        currentState = KittyState.Running;
        lastState = currentState;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != lastState)
        {
            switch(currentState)
            {
                case KittyState.Running:
                    anim.AnimationState.SetAnimation(0, "Run", true);
                    anim.timeScale = 1;
                    break;

                case KittyState.Jumping:
                    anim.AnimationState.SetAnimation(0, "Jump", true);
                    anim.timeScale = 1;
                    break;
            }

            lastState = currentState;
        }

        if (isRight)
        {
            transform.localScale = new Vector3(1,1,1);
            transform.Translate(new Vector3(speed * Time.deltaTime, 0,0));
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }

        if (transform.position.x < -13.8f || transform.position.x > 13.8f)
        {
            if (isRight)
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }
            else
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
                isRight = !isRight;
        }

        if(jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }
        else
        {
            rb.AddForce(new Vector2(0,9),ForceMode2D.Impulse);
            currentState = KittyState.Jumping;
            jumpTimer = UnityEngine.Random.Range(1.6f, 8f);
        }

        if (onFloor)
        {
            currentState = KittyState.Running;
        }
        else
        {
            currentState = KittyState.Jumping;
        }

        if(Health <= 0)
        {
            KittyDied?.Invoke();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        damage = damage;
        Health -= 1;
        StartCoroutine(HurtRoutine());
    }

    IEnumerator HurtRoutine()
    {
        anim.skeleton.SetColor(Color.red);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.white);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onFloor = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onFloor = false;
        }
    }


}
