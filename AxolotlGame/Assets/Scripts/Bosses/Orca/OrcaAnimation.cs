using UnityEngine;
using Spine.Unity;
using System.Collections;
using Unity.Cinemachine;

public class OrcaAnimation : MonoBehaviour
{
    public SkeletonAnimation anim;
    OrcaMovement orcaMovement;
    public CinemachineImpulseSource impulseSource;
    public ParticleSystem LandParticle;

    public Transform diveLocation1;
    public Transform diveLocation2;

    public GameObject attackHitbox;
    public GameObject diveAttackHitbox;

    public Transform playerLoc;
    public Vector3 playerCurrentLoc;

    private void Start()
    {
        orcaMovement = GetComponent<OrcaMovement>();
        playerLoc = GameObject.Find("Axolotl").GetComponent<Transform>();
    }

    private void Update()
    {
        if (orcaMovement.currentState != orcaMovement.lastState)
        {
            switch(orcaMovement.currentState)
            {
                case OrcaMovement.OrcaState.idle:
                    anim.AnimationState.SetAnimation(0, "Idle", true);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    anim.timeScale = 0.2f;
                    orcaMovement.normalAttackTimer = 1;
                    break;

                case OrcaMovement.OrcaState.walking:
                    anim.AnimationState.SetAnimation(0, "Walk", true);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    anim.timeScale = 0.4f;
                    break;

                case OrcaMovement.OrcaState.charging:
                    if(orcaMovement.currentAttackType == OrcaMovement.AttackType.Attack)
                    {
                        StartCoroutine(NormalAttack());
                    }
                    else if(orcaMovement.currentAttackType == OrcaMovement.AttackType.DashAttack)
                    {
                        StartCoroutine(DashAttack());
                    }
                    else if(orcaMovement.currentAttackType == OrcaMovement.AttackType.DiveAttack)
                    {
                        StartCoroutine(DiveAttack());
                    }
                        break;
            }
            orcaMovement.lastState = orcaMovement.currentState;
        }
    }

    void addForce(int force)
    {
        
        if (orcaMovement.right)
        {
            orcaMovement.rb.AddForce(new Vector2(force, 0), ForceMode2D.Impulse);
        }
        else
        {
            orcaMovement.rb.AddForce(new Vector2(-force, 0), ForceMode2D.Impulse);
        }
    }

    IEnumerator NormalAttack()
    {
        anim.timeScale = 1f;
        anim.AnimationState.SetAnimation(0, "Attack Charge", false);
        yield return new WaitForSeconds(0.4f);
        anim.AnimationState.SetAnimation(0, "Attack", false);
        addForce(20);
        orcaMovement.currentState = OrcaMovement.OrcaState.attacking;
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);
        orcaMovement.rb.linearVelocityX = 0;
        yield return new WaitForSeconds(0.4f);
        if(orcaMovement.currentState == OrcaMovement.OrcaState.attacking)
        {
            orcaMovement.currentState = OrcaMovement.OrcaState.idle;
        }
    }

    IEnumerator DashAttack()
    {
        anim.timeScale = 1f;
        anim.AnimationState.SetAnimation(0, "Dash Charge", false);
        yield return new WaitForSeconds(0.4f);
        anim.AnimationState.SetAnimation(0, "Dash", false);
        attackHitbox.SetActive(true);
        addForce(80);
        orcaMovement.currentState = OrcaMovement.OrcaState.attacking;
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);
        CameraShake(1f);
        yield return new WaitForSeconds(0.2f);
        orcaMovement.rb.linearVelocityX = 0;
        yield return new WaitForSeconds(0.1f);
        orcaMovement.currentState = OrcaMovement.OrcaState.idle;
    }

    IEnumerator DiveAttack()
    {
        Vector2 direction = Vector2.zero;
        anim.timeScale = 1f;
        anim.AnimationState.SetAnimation(0,"Dive", false);
        while (transform.position.y < 25)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,transform.position.y +20, transform.position.z),80*Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector3(Random.Range(diveLocation1.position.x, diveLocation2.position.x), diveLocation1.position.y, 0);
        yield return new WaitForSeconds(0.5f);
        diveAttackHitbox.SetActive(true);
        orcaMovement.currentState = OrcaMovement.OrcaState.attacking;
        playerCurrentLoc = playerLoc.transform.position;
        direction = playerCurrentLoc - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        while (transform.position.y > -3.651838)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerCurrentLoc.x, -4f,0), 100 * Time.deltaTime);
            yield return null;
        }
        LandParticle.Emit(25);
        CameraShake(1f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = new Vector3(transform.position.x, -8.527193f,0);
        anim.AnimationState.SetAnimation(0, "Dash Charge", false);
        diveAttackHitbox.SetActive(false);
        yield return new WaitForSeconds(1f);
        orcaMovement.currentState = OrcaMovement.OrcaState.idle;



    }

    void CameraShake(float power)
    {
        impulseSource.GenerateImpulse(power);
    }


}
