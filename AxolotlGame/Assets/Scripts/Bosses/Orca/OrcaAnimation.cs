using Spine.Unity;
using System.Collections;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrcaAnimation : MonoBehaviour
{
    public SkeletonAnimation anim;
    public GameObject darkCircle;
    public OrcaMovement orcaMovement;
    public BossHealth orcaHealth;
    public CinemachineImpulseSource impulseSource;
    public ParticleSystem LandParticle;
    public GameObject deathParticle;

    public Transform diveLocation1;
    public Transform diveLocation2;

    public GameObject attackHitbox;
    public GameObject diveAttackHitbox;
    public GameObject lightWarner;
    public GameObject lightAttack;
    public GameObject cactusRight;
    public GameObject cactusLeft;
    public GameObject cactusUp;

    public Transform playerLoc;
    public Vector3 playerCurrentLoc;
    public AudioSource bgSource;

    private void Start()
    {
        orcaMovement = GetComponent<OrcaMovement>();
        orcaHealth = GetComponent<BossHealth>();
        playerLoc = GameObject.Find("Axolotl").GetComponent<Transform>();
    }

    private void Update()
    {
        if (orcaMovement.currentState != orcaMovement.lastState && orcaMovement.phase == 1)
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

        if(orcaMovement.phase == 0 && (orcaMovement.currentState != OrcaMovement.OrcaState.attacking && orcaMovement.currentState != OrcaMovement.OrcaState.charging))
        {
            StartCoroutine(PhaseTransition());
        }

        if (orcaMovement.phase == 2 && orcaHealth.Health <= 0)
        {
            orcaMovement.phase = 100;
            StopAllCoroutines();
            StartCoroutine(DeadRoutine());
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
        while (transform.position.y > -3.551838)
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

    public IEnumerator PhaseTransition()
    {
        orcaMovement.phase = -1;
        anim.timeScale = 5f;
        anim.AnimationState.SetAnimation(0,"Stunned",false);
        CameraShake(3f);
        yield return new WaitForSeconds(4f);
        darkCircle.SetActive(true);
        while (darkCircle.transform.localScale.x < 79)
        {
            darkCircle.transform.localScale = Vector3.Lerp(darkCircle.transform.localScale, new Vector3(80, 80, 80), 8f * Time.deltaTime);
            yield return null;
        }
        
        yield return new WaitForSeconds(2f);
        orcaHealth.Health = 400;
        transform.position = Vector3.zero;
        orcaMovement.rb.bodyType = RigidbodyType2D.Kinematic;
        anim.Skeleton.SetSkin("front");
        anim.Skeleton.SetSlotsToSetupPose();
        anim.AnimationState.Apply(anim.Skeleton);
        anim.timeScale = 0.4f;
        anim.AnimationState.SetAnimation(0, "Front Flying Idle", true);
        while (darkCircle.transform.localScale.x > 0.1)
        {
            darkCircle.transform.localScale = Vector3.Lerp(darkCircle.transform.localScale, Vector3.zero, 10f * Time.deltaTime);
            yield return null;
        }

        darkCircle.SetActive(false);

        yield return new WaitForSeconds(2f);
        orcaMovement.phase = 2;

        StartCoroutine(phase2Routine());
    }

    IEnumerator LightAttackRoutine()
    {
        lightWarner.SetActive(true);
        yield return new WaitForSeconds(1f);
        lightWarner.SetActive(false);
        CameraShake(2f);
        lightAttack.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        lightAttack.SetActive(false);

    }

    IEnumerator TeleportAttackRoutine()
    {
        StartCoroutine(TPRoutine());
        yield return new WaitForSeconds(1.7f);
        StartCoroutine(TPRoutine());
        yield return new WaitForSeconds(1.7f);
        StartCoroutine(TPRoutine());
        yield return new WaitForSeconds(1.7f);
        StartCoroutine(TPRoutine());
        yield return new WaitForSeconds(1.7f);
        anim.skeleton.SetColor(Color.black);
        yield return new WaitForSeconds(0.06f);
        transform.position = new Vector3(0, 15, 0);
        yield return new WaitForSeconds(1f);
        anim.skeleton.SetColor(Color.white);
        anim.AnimationState.SetAnimation(0, "Front Flying Idle", true);
        transform.position = Vector3.zero;
    }

    IEnumerator TPRoutine()
    {
        diveAttackHitbox.SetActive(true);
        anim.skeleton.SetColor(Color.black);
        yield return new WaitForSeconds(0.06f);
        transform.position = new Vector3(0, 15, 0);
        yield return new WaitForSeconds(0.5f);
        playerCurrentLoc = playerLoc.position;
        transform.position = new Vector3(playerCurrentLoc.x, 1, 0);
        anim.AnimationState.SetAnimation(0, "Front TP Charge", false);
        yield return new WaitForSeconds(0.07f);
        anim.skeleton.SetColor(Color.white);
        yield return new WaitForSeconds(0.4f);
        while (transform.position.y > -8.9)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -9f, 0), 100 * Time.deltaTime);
            yield return null;
        }
        diveAttackHitbox.SetActive(false);
        CameraShake(1f);
    }

    IEnumerator CactusRightRoutine()
    {
        while (cactusRight.transform.position.x > 44.3)
        {
            cactusRight.transform.position = Vector3.MoveTowards(cactusRight.transform.position, new Vector3(44, cactusRight.transform.position.y, 0), 100 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.8f);
        while (cactusRight.transform.position.x > 15.3)
        {
            cactusRight.transform.position = Vector3.MoveTowards(cactusRight.transform.position, new Vector3(15, cactusRight.transform.position.y, 0), 100 * Time.deltaTime);
            yield return null;
        }
        CameraShake(1f);
        yield return new WaitForSeconds(0.1f);
        while (cactusRight.transform.position.x < 47.8)
        {
            cactusRight.transform.position = Vector3.MoveTowards(cactusRight.transform.position, new Vector3(48, cactusRight.transform.position.y, 0), 100 * Time.deltaTime);
            yield return null;
        }
        
    }

    IEnumerator CactusLeftRoutine()
    {
        while (cactusLeft.transform.position.x < -45.3)
        {
            cactusLeft.transform.position = Vector3.MoveTowards(cactusLeft.transform.position, new Vector3(-45, cactusLeft.transform.position.y, 0), 100 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.8f);
        while (cactusLeft.transform.position.x < -15.3)
        {
            cactusLeft.transform.position = Vector3.MoveTowards(cactusLeft.transform.position, new Vector3(-15, cactusLeft.transform.position.y, 0), 100 * Time.deltaTime);
            yield return null;
        }
        CameraShake(1f);
        yield return new WaitForSeconds(0.1f);
        while (cactusLeft.transform.position.x > -48.8)
        {
            cactusLeft.transform.position = Vector3.MoveTowards(cactusLeft.transform.position, new Vector3(-49, cactusLeft.transform.position.y, 0), 100 * Time.deltaTime);
            yield return null;
        }
        
    }

    IEnumerator CactusUpRoutine()
    {
        playerCurrentLoc = playerLoc.transform.position;
        cactusUp.transform.position = new Vector3(playerCurrentLoc.x - 3.5f, cactusUp.transform.position.y, 0);
        while (cactusUp.transform.position.y > 35.3)
        {
            cactusUp.transform.position = Vector3.MoveTowards(cactusUp.transform.position, new Vector3(cactusUp.transform.position.x, 35, 0), 100 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.4f);
        while (cactusUp.transform.position.y > 19.3)
        {
            cactusUp.transform.position = Vector3.MoveTowards(cactusUp.transform.position, new Vector3(cactusUp.transform.position.x, 19, 0), 100 * Time.deltaTime);
            yield return null;
        }
        CameraShake(2f);
        yield return new WaitForSeconds(0.1f);
        while (cactusUp.transform.position.y < 43.7)
        {
            cactusUp.transform.position = Vector3.MoveTowards(cactusUp.transform.position, new Vector3(cactusUp.transform.position.x, 44, 0), 100 * Time.deltaTime);
            yield return null;
        }
        
    }

    IEnumerator DeadRoutine()
    {
        GameManager.finishTime = GameManager.counter;
        bgSource.time = 131.5f;
        cactusLeft.SetActive(false);
        cactusRight.SetActive(false);
        cactusUp.SetActive(false);
        lightWarner.SetActive(false);
        lightAttack.SetActive(false);
        orcaMovement.rb.bodyType = RigidbodyType2D.Dynamic;
        orcaMovement.rb.linearVelocityY = -40;
        CameraShake(2f);
        anim.Skeleton.SetSkin("side");
        anim.Skeleton.SetSlotsToSetupPose();
        anim.AnimationState.Apply(anim.Skeleton);
        anim.timeScale = 1f;
        anim.AnimationState.SetAnimation(0, "Death 1", true);
        yield return new WaitForSeconds(4);
        anim.AnimationState.SetAnimation(0, "Death 2", true);
        yield return new WaitForSeconds(0.2f);
        deathParticle.SetActive(true);
        darkCircle.SetActive(true);
        yield return new WaitForSeconds(10f);
        CameraShake(2f);
        while (darkCircle.transform.localScale.x < 79)
        {
            darkCircle.transform.localScale = Vector3.Lerp(darkCircle.transform.localScale, new Vector3(80, 80, 80), 8f * Time.deltaTime);
            yield return null;
        }
        CameraShake(2f);
        yield return new WaitForSeconds(0.4f);
        bgSource.Stop();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameEnd");
    }

    // 2.Faz Rutini
    public IEnumerator phase2Routine()
    {
        StartCoroutine(LightAttackRoutine());
        yield return new WaitForSeconds(2);
        StartCoroutine(LightAttackRoutine());
        yield return new WaitForSeconds(3);
        StartCoroutine(TeleportAttackRoutine());
        yield return new WaitForSeconds(9);
        StartCoroutine(CactusRightRoutine());
        yield return new WaitForSeconds(2f);
        StartCoroutine(CactusLeftRoutine());
        yield return new WaitForSeconds(2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(3f);
        StartCoroutine(TeleportAttackRoutine());
        yield return new WaitForSeconds(6f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(3f);
        StartCoroutine(LightAttackRoutine());
        yield return new WaitForSeconds(4f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(4f);
        StartCoroutine(LightAttackRoutine());
        yield return new WaitForSeconds(4f);
        StartCoroutine(TeleportAttackRoutine());
        yield return new WaitForSeconds(9f);
        StartCoroutine(CactusRightRoutine());
        yield return new WaitForSeconds(2f);
        StartCoroutine(LightAttackRoutine());
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(LightAttackRoutine());
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(LightAttackRoutine());
        yield return new WaitForSeconds(4f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusRightRoutine());
        StartCoroutine(CactusLeftRoutine());
        yield return new WaitForSeconds(3f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CactusUpRoutine());
        yield return new WaitForSeconds(3f);

        StartCoroutine(phase2Routine());
    }


}
