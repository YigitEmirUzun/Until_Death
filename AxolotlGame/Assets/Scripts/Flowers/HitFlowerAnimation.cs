using UnityEngine;
using Spine.Unity;

public class HitFlowerAnimation : MonoBehaviour,IDamageable
{
    public SkeletonAnimation spineAnim;
    void Start()
    {
        spineAnim.AnimationState.SetAnimation(0, "Idle", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        spineAnim.AnimationState.SetAnimation(0, "On Hit", false);
        spineAnim.AnimationState.AddAnimation(0,"Idle",true,0.2f);
    }
}
