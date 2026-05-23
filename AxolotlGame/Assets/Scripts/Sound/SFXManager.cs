using System;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource sfxSource;

    public AudioClip dashClip;
    public AudioClip jumpClip;
    public AudioClip AttackClip;
    public AudioClip HitClip;
    void OnEnable()
    {
        PlayerAnimations.OnAttack += AttackSound;
        PlayerAnimations.OnDash += DashSound;
        PlayerAnimations.OnJump += JumpSound;
        AttackHitbox.OnHit += HitSound;
    }

    void OnDisable()
    {
        PlayerAnimations.OnAttack -= AttackSound;
        PlayerAnimations.OnDash -= DashSound;
        PlayerAnimations.OnJump -= JumpSound;
        AttackHitbox.OnHit -= HitSound;
    }

    public void DashSound()
    {
        sfxSource.PlayOneShot(dashClip);
    }

    public void JumpSound()
    {
        sfxSource.PlayOneShot(jumpClip);
    }

    public void AttackSound()
    {
        sfxSource.PlayOneShot(AttackClip);
    }

    public void HitSound()
    {
        sfxSource.PlayOneShot(HitClip);
    }
}
