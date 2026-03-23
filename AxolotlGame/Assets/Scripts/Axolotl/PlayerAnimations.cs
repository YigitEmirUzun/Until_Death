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
        attack
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

        if (playerController.Attacking)
        {
            currentState = PlayerState.attack;
        }

        if (playerController.moving && floorDetector.OnFloor && !playerController.Attacking)
        {
            currentState = PlayerState.run;
        }

        if (!playerController.moving && floorDetector.OnFloor && !playerController.Attacking)
        {
            currentState = PlayerState.idle;
        }



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
                    spineAnim.AnimationState.SetAnimation(0, "Attack", false);
                    StartCoroutine(AnimationDelay(0f, 0.5f, PlayerState.attack));
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
