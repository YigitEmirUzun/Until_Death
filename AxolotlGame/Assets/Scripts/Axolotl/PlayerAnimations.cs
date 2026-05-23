using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;


public class PlayerAnimations : MonoBehaviour
{
    public SkeletonAnimation spineAnim;

    FloorDetector floorDetector;
    PlayerController playerController;
    WallDetector wallDetector;

    public static Action OnJump;
    public static Action OnDash;
    public static Action OnAttack;

    

    enum PlayerState
    {
        idle,
        run,
        jump,
        fall,
        attack,
        dash,
        wall
    }

    PlayerState currentState;
    PlayerState lastState;

    void Awake()
    {
        floorDetector = GetComponentInChildren<FloorDetector>();
        wallDetector = GetComponentInChildren<WallDetector>();
        playerController = GetComponent<PlayerController>();

        
    }

    private void Start()
    {
        spineAnim.AnimationState.SetAnimation(0, "Fall", false);
        currentState = PlayerState.fall;
        lastState = PlayerState.run;
    }

    void Update()
    {
        if (DialogueManager.Instance.isDialogue)
        {
            currentState = PlayerState.idle;
        }
        else
        {
            if (playerController.isDashing)
            {
                currentState = PlayerState.dash;
            }
            else if (playerController.Attacking)
            {
                currentState = PlayerState.attack;
            }
            else if (wallDetector.onWall && !floorDetector.OnFloor)
            {
                currentState = PlayerState.wall;
            }
            else if (playerController.moving && floorDetector.OnFloor)
            {
                currentState = PlayerState.run;
            }
            else if (!playerController.moving && floorDetector.OnFloor)
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
                    AttackAnimation();
                    OnAttack?.Invoke();
                    break;

                case PlayerState.jump:
                    spineAnim.AnimationState.SetAnimation(0, "Jump", false);
                    OnJump?.Invoke();
                    StartCoroutine(AnimationDelay(0f, 1f, PlayerState.jump));
                    break;

                case PlayerState.fall:
                    spineAnim.AnimationState.SetAnimation(0, "Fall", false);
                    StartCoroutine(AnimationDelay(0f, 0.5f, PlayerState.fall));
                    break;

                case PlayerState.dash:
                    spineAnim.AnimationState.SetAnimation(0, "Dash", false);
                    OnDash?.Invoke();
                    StartCoroutine(AnimationDelay(0f, 2f, PlayerState.dash));
                    break;

                case PlayerState.wall:
                    spineAnim.AnimationState.SetAnimation(0,"Wall", false);
                    StartCoroutine(AnimationDelay(0f, 1f, PlayerState.dash));
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

    void AttackAnimation()
    {
        switch (playerController.currentDirection)
        {
            case PlayerController.DirectionState.Horizontal:
                spineAnim.AnimationState.SetAnimation(0, "Attack Sideways", false);
                StartCoroutine(AnimationDelay(0f, 1f, PlayerState.attack));
                break;
            case PlayerController.DirectionState.Up:
                spineAnim.AnimationState.SetAnimation(0, "Attack Up", false);
                StartCoroutine(AnimationDelay(0f, 1f, PlayerState.attack));
                break;
            case PlayerController.DirectionState.Down:
                spineAnim.AnimationState.SetAnimation(0, "Attack Down", false);
                StartCoroutine(AnimationDelay(0f, 1f, PlayerState.attack));
                break;
        }
    }

}
