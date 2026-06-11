using Spine.Unity;
using System.Collections;
using UnityEngine;

public class FishMovement : MonoBehaviour, IDamageable
{
    public SkeletonAnimation anim;
    public FishFloorDetector floorDetector;
    public FishRangeDetector rangeDetector;

    public GameObject attackHitbox;

    public bool isRight = true;
    
    public GameObject player;

    public FishState currentState;
    public FishState lastState;

    public enum FishState
    {
        Running,
        Attacking
    }

    private void Awake()
    {
        currentState = FishState.Running;
        lastState = FishState.Running;
        rangeDetector = GetComponentInChildren<FishRangeDetector>();
        floorDetector = GetComponentInChildren<FishFloorDetector>();
        player = GameObject.Find("Axolotl");
        anim = GetComponent<SkeletonAnimation>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(currentState != lastState)
        {
            switch (currentState)
            {
                case FishState.Running:
                    anim.AnimationState.SetAnimation(0, "Run", true);
                    break;

                case FishState.Attacking:
                    anim.AnimationState.SetAnimation(0, "Attack", false);
                    StartCoroutine(AttackRoutine());
                    break;
            }

            lastState = currentState;

        }
        
        
        if (!floorDetector.onFloor || rangeDetector.onWall)
        {
            isRight = !isRight;


        }

        if(rangeDetector.onRange)
        {
            if(currentState != FishState.Attacking)
            {
                if(player.transform.position.x > transform.position.x)
                {
                    isRight = true;
                }
                else { isRight = false; }
            }
            currentState = FishState.Attacking;
        }
        
        if (currentState == FishState.Running)
        {
            if(isRight)
            {
                transform.localScale = new Vector3(1,1,1);
                transform.Translate(new Vector3(6 * Time.deltaTime, 0, 0));
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.Translate(new Vector3(-6 * Time.deltaTime, 0, 0));
            }
        }
        

    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        attackHitbox.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        currentState = FishState.Running;
    }

    public void TakeDamage(int damage)
    {
        Destroy(gameObject);
    }
}
