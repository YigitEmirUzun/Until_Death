using Spine.Unity;
using UnityEngine;

public class CatBossMovement : MonoBehaviour
{
    SkeletonAnimation anim;
    Rigidbody2D rb;

    public bool isDead = false;

    public CatState currentState;
    public CatState lastState;

    public bool isRight;
    public bool onFloor;
    public bool stunned = false;

    public float speed;
    public float jumpTimer;
    public float stunnedTimer;

    public float health;

    BossHealth bossHealth;

    public enum CatState
    {
        Running,
        Jumping,
        Stunned
    }

    private void Awake()
    {
        
        bossHealth = GetComponentInChildren<BossHealth>();
        bossHealth.Health = health;
        anim = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>();
        speed = 11;
        jumpTimer = UnityEngine.Random.Range(3f, 12f);

        
    }
    void Start()
    {
        stunnedTimer = 4;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != lastState)
        {
            switch (currentState)
            {
                case CatState.Running:
                    anim.AnimationState.SetAnimation(0, "Run", true);
                    anim.timeScale = 1;
                    break;

                case CatState.Jumping:
                    anim.AnimationState.SetAnimation(0, "Jump", false);
                    anim.timeScale = 1;
                    break;
                
                case CatState.Stunned:
                    anim.AnimationState.SetAnimation(0, "Fall", false);
                    anim.timeScale = 1;
                    break;
            }

            lastState = currentState;
        }

        if (isRight)
        {
            if (currentState != CatState.Stunned && !isDead)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
        }
        else
        {
            if(currentState != CatState.Stunned && !isDead)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }
        }

        if (transform.position.x < -14.8f || transform.position.x > 14.8f)
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

        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }
        else
        {
            if(!stunned && !isDead)
            {
                rb.AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
                currentState = CatState.Jumping;
                jumpTimer = UnityEngine.Random.Range(1.6f, 8f);
            }
        }

        if (stunned)
        {
            rb.linearVelocityY = -20;
            currentState = CatState.Stunned;
            if(stunnedTimer > 0)
            {
                stunnedTimer -= Time.deltaTime;
            }
            else
            {
                stunned = false;
                currentState = CatState.Running;
                stunnedTimer = 4;
            }
        }

        if (onFloor && !stunned && !isDead)
        {
            currentState = CatState.Running;
        }

        if(!onFloor  && !stunned && !isDead)
        {
            currentState = CatState.Jumping;
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("IceBall"))
        {
            stunned = true;
            Destroy(collision.gameObject);
            Debug.Log("Buz ile vuruldu");
        }
    }
}
