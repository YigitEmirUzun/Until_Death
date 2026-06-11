using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KittyCounter : MonoBehaviour
{
    public float KittyCount = 8;
    int phase;
    int lastPhase;
    public GameObject kittyPrefab;
    public GameObject IceBallPrefab;
    public Rigidbody2D CatBossRb;
    public CatBossMovement CatBossSC;
    public BossHealth bossHealth;
    public AxolotlHealth axolotlHealth;

    public SpriteRenderer sr;
    UnityEngine.Color color;
    public GameObject darkScreen;

    public float IceBallTimer;
    void Start()
    {
        color = sr.color;
        CatBossSC.enabled = false;
        IceBallTimer = Random.Range(10, 18);
        phase = 1;
        lastPhase = phase;
    }

    private void OnEnable()
    {
        KittyMovement.KittyDied += KittyDead;
    }

    // Update is called once per frame
    void Update()
    {
        if(KittyCount == 4)
        {
            phase = 2;
        }

        if(KittyCount == 0)
        {
            phase = 3;
        }

        if(lastPhase != phase)
        {
            if (phase == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    float xPosition = Random.Range(-10, 10);
                    Instantiate(kittyPrefab,new Vector3(xPosition, 11,0), Quaternion.identity);
                }
            }
            if(phase == 3)
            {
                CatBossRb.bodyType = RigidbodyType2D.Dynamic;
                CatBossSC.enabled = true;
            }

            lastPhase = phase;
        }

        if(phase == 3)
        {
            if (IceBallTimer > 0)
            {
                IceBallTimer -= Time.deltaTime;
            }
            else
            {
                float xPosition = Random.Range(-10, 10);
                Instantiate(IceBallPrefab, new Vector3(xPosition, 12, 0), Quaternion.identity);
                IceBallTimer = Random.Range(10, 18);
            }

            if(bossHealth.Health <= 0)
            {
                CatBossSC.stunned = true;
                CatBossSC.isDead = true;

                axolotlHealth.currentHealth = axolotlHealth.maxHealth;
                color = sr.color;

                if (color.a < 1f)
                {
                    color.a += 0.3f * Time.deltaTime;
                    sr.color = color;
                }
                else
                {
                    StartCoroutine(NextSceneRoutine());
                }

            }
        }
    }

    IEnumerator NextSceneRoutine()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Heaven");
    }

    void KittyDead()
    {
        KittyCount--;
    }

}
