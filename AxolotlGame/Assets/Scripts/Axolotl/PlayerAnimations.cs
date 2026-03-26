using Spine.Unity;
using System.Collections;
using UnityEngine;


public class PlayerAnimations : MonoBehaviour
{
    public SkeletonAnimation spineAnim;

    FloorDetector floorDetector;
    PlayerController playerController;

    enum PlayerState
    {
        idle,
        run,
        jump,
        fall,
        attack,
        dash
    }

    PlayerState currentState;
    PlayerState lastState;

    void Awake()
    {
        floorDetector = GetComponentInChildren<FloorDetector>();
        playerController = GetComponent<PlayerController>();

        currentState = PlayerState.idle;
        lastState = PlayerState.run;
    }

    void Update()
    {

        if (playerController.isDashing)
        {
            currentState = PlayerState.dash;
        }
        if (playerController.Attacking && !playerController.isDashing)
        {
            currentState = PlayerState.attack;
        }
        else if (playerController.moving && floorDetector.OnFloor && !playerController.Attacking && !playerController.isDashing)
        {
            currentState = PlayerState.run;
        }
        else if (!playerController.moving && floorDetector.OnFloor && !playerController.Attacking && !playerController.isDashing)
        {
            currentState = PlayerState.idle;
        }
        else if (playerController.Jumping && !playerController.isDashing)
        {
            currentState = PlayerState.jump;
        }
        else if (!floorDetector.OnFloor && !playerController.Jumping && !playerController.Attacking && !playerController.isDashing)
        {
            currentState = PlayerState.fall;
        }




        // Animasyon Degisimi
        if (currentState != lastState)
        {
            switch (currentState)
            {
                case PlayerState.idle:
                    spineAnim.AnimationState.SetAnimation(0, "Idle", true);
                    StartCoroutine(AnimationDelay(0.2f, 0.1f, PlayerState.idle));
                    break;

                case PlayerState.run:
                    spineAnim.AnimationState.SetAnimation(0, "Run", true);
                    StartCoroutine(AnimationDelay(0f, 0.8f, PlayerState.run));
                    break;

                case PlayerState.attack:
                    spineAnim.AnimationState.SetAnimation(0, "Attack Sideways", false);
                    StartCoroutine(AnimationDelay(0f, 1f, PlayerState.attack));
                    break;

                case PlayerState.jump:
                    spineAnim.AnimationState.SetAnimation(0, "Jump", false);
                    StartCoroutine(AnimationDelay(0f, 1f, PlayerState.jump));
                    break;

                case PlayerState.fall:
                    spineAnim.AnimationState.SetAnimation(0, "Fall", false);
                    StartCoroutine(AnimationDelay(0f, 0.5f, PlayerState.fall));
                    break;

                case PlayerState.dash:
                    spineAnim.AnimationState.SetAnimation(0, "Dash", false);
                    StartCoroutine(AnimationDelay(0f, 2f, PlayerState.dash));
                    break;
            }

            lastState = currentState;
        }


    }

    IEnumerator AnimationDelay(float second , float scale, PlayerState state)
    {
        yield return new WaitForSeconds(second);
        if(currentState == state) { spineAnim.timeScale = scale; }
        

    }

}
