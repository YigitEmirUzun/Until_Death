using Spine.Unity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrcaMovement : MonoBehaviour
{
    GameObject player;
    BossRangeDetector detector;
    BossHealth orcaHealth;
    public Rigidbody2D rb;
    public int phase = 1;

    public float speed = 5;

    bool playerInRange = false;
    public bool right = false;

    public OrcaState currentState = OrcaState.idle;
    public OrcaState lastState = OrcaState.idle;

    public AttackType currentAttackType = AttackType.Attack;
    public List<AttackType> specialAttackType;

    float walkingTimer = 3;
    public float normalAttackTimer = 0.2f;
    float specialAttackTimer = 9;
    
    public enum OrcaState
    {
        idle,
        walking,
        attacking,
        charging
    }

    public enum AttackType
    {
        Attack,
        DashAttack,
        DiveAttack
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Axolotl");
        detector = GetComponentInChildren<BossRangeDetector>();
        orcaHealth = GetComponent<BossHealth>();
        specialAttackType.Add(AttackType.DashAttack);
        specialAttackType.Add(AttackType.DiveAttack);
    }

    // Update is called once per frame
    void Update()
    {
        if(phase == 1)
        {
            playerInRange = detector.playerInRange;
            Walking();
            Attack();
        }

        if(orcaHealth.Health <= 0 && phase == 1)
        {
            phase = 0;
        }
    }

    private void FixedUpdate()
    {
        if(phase == 1)
        {
            Direction();
        }
    }

    void Direction()
    {
        if(currentState != OrcaState.attacking && currentState != OrcaState.charging)
        {
            if(player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                right = false;
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                right = true;
            }
        }
    }

    void Walking()
    {
        if (currentState != OrcaState.attacking && currentState != OrcaState.charging && !playerInRange)
        {
            if(walkingTimer <= 0)
            {
                currentState = OrcaState.walking;
                walkingTimer = Random.Range(2, 5);
            }
            else
            {
                walkingTimer -= Time.deltaTime;
            }
        }

        if(currentState == OrcaState.walking)
        {
            if(player.transform.position.x < transform.position.x - 2.5f)
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }
            else if(player.transform.position.x > transform.position.x + 2.5f)
            {
                transform.Translate(new Vector3(+speed * Time.deltaTime, 0, 0));
            }
        }
    }

    void Attack()
    {
        if(currentState != OrcaState.attacking && currentState != OrcaState.charging && specialAttackTimer > 0)
        {
            if(normalAttackTimer <= 0)
            {
                currentAttackType = AttackType.Attack;
                currentState = OrcaState.charging;
                normalAttackTimer = Random.Range(0.2f, 1f);
            }
            
            if (playerInRange)
            {
                normalAttackTimer -= Time.deltaTime;
            }
            else
            {
                normalAttackTimer = Random.Range(0.5f, 1f);
            }
        }

        if (specialAttackTimer <= 0)
        {
            if(currentState != OrcaState.attacking && currentState != OrcaState.charging)
            {
                normalAttackTimer = 0.4f;
                currentAttackType = specialAttackType[Random.Range(0, specialAttackType.Count)];
                currentState = OrcaState.charging;
                specialAttackTimer = Random.Range(2, 5);
            } 
        }
        else { specialAttackTimer -= Time.deltaTime; }
    }


}
