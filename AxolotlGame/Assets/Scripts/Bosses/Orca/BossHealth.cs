using Spine.Unity;
using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour, IDamageable
{
    public float Health = 1000;
    public SkeletonAnimation anim;
    public ParticleSystem bloodParticle;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        bloodParticle.Emit(8);
        StartCoroutine(HurtRoutine());
    }

    IEnumerator HurtRoutine()
    {
        anim.skeleton.SetColor(Color.red);
        yield return new WaitForSeconds(0.1f);
        anim.skeleton.SetColor(Color.white);
    }
}
