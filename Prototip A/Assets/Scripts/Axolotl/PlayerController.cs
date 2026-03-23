using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Spine.Unity;

public class PlayerController : MonoBehaviour
{

    public SkeletonAnimation spineAnim;

    InputSystem_Actions action;
    Rigidbody2D rb;
    FloorDetector floorDetector;
    HangDetector hangDetector;

    public Transform UpAttackPosition;
    public Transform DownAttackPosition;
    public Transform AttackPosition;
    bool IsRight = true;
    bool Hang = false;
    public bool moving;
    public bool Attacking = false;
    public bool canAttack = true;

    public GameObject VerticalAttack;
    public GameObject HorizontalAttack;

    public Vector2 moveInput;
    public float speed;
    public float JumpForce;
   
    void Awake()
    {
        action = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        floorDetector = GetComponentInChildren<FloorDetector>();
        hangDetector = GetComponentInChildren<HangDetector>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(moveInput.x);
    }

    private void FixedUpdate()
    {
        if(moveInput.x >= 0.6 || moveInput.x <= -0.6)
        {
            rb.linearVelocityX = moveInput.x * speed;
            moving = true;
        }
        else { moving = false; }

            DirectionControl();



        if (Hang)
        {
            if(moveInput.y > 0.6)
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(moveInput.x * 30, 30), ForceMode2D.Impulse);
                Hang = false;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(moveInput.x * 30, moveInput.y * 30), ForceMode2D.Impulse);
                Hang = false;
            }
            
        }
    }

    private void OnEnable()
    {
        action.Player.Enable();

        action.Player.Move.performed += OnMovePerformed;
        action.Player.Move.canceled += OnMoveCanceled;
        action.Player.Dash.performed += OnDashPerformed;
        action.Player.Hang.performed += OnHangPerformed;
        action.Player.Hang.canceled += OnHangCanceled;
        action.Player.Jump.performed += OnJumpPerformed;
        action.Player.Attack.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        action.Player.Disable();

        action.Player.Move.performed -= OnMovePerformed;
        action.Player.Move.canceled -= OnMoveCanceled;
        action.Player.Dash.performed -= OnDashPerformed;
        action.Player.Hang.performed -= OnHangPerformed;
        action.Player.Hang.canceled -= OnHangCanceled;
        action.Player.Jump.performed -= OnJumpPerformed;
        action.Player.Attack.performed -= OnAttackPerformed;
    }

    void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
        if (floorDetector.OnFloor)
        {
            if (IsRight) { rb.linearVelocityX = 5f; }
            else { rb.linearVelocityX = -5f; }
        }
        else { rb.linearVelocityX = 0; }
    }

    void OnDashPerformed(InputAction.CallbackContext context)
    {
        rb.AddForce(new Vector2(25, 0), ForceMode2D.Impulse);
        StartCoroutine(DashRoutine(0.2f));
    }

    IEnumerator DashRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        rb.linearVelocityX = 0f;
    }
    void OnHangPerformed(InputAction.CallbackContext context)
    {
        if (hangDetector.canHang)
        {
            Time.timeScale = 0.1f;

        }
    }

    void OnHangCanceled(InputAction.CallbackContext context)
    {
        if (hangDetector.canHang)
        {
            Time.timeScale = 1f;
            rb.linearVelocity = Vector2.zero;
            Hang = true;
        }
    }

    void OnAttackPerformed(InputAction.CallbackContext context)
    {
 
        if (canAttack)
        {
            Attacking = true;
            canAttack = false; 
            rb.linearVelocityX = 0;
            StartCoroutine(AttackRoutine());
            if (moveInput.y > 0.4f)
            {
                StartCoroutine(AttackColliderRoutine(HorizontalAttack, UpAttackPosition));
            }
            else if (moveInput.y < -0.4)
            {
                StartCoroutine(AttackColliderRoutine(HorizontalAttack, DownAttackPosition));
            }
            else { StartCoroutine(AttackColliderRoutine(VerticalAttack, AttackPosition)); }
        }
        
    }

    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (floorDetector.OnFloor)
        {
            rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }
    }

    IEnumerator AttackColliderRoutine(GameObject AttackObject, Transform AttackPosition)
    {
        GameObject NewAttack = Instantiate(AttackObject, AttackPosition.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        GameObject.Destroy(NewAttack);
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.4f);
        Attacking = false;
        yield return new WaitForSeconds(0.2f);
        canAttack = true;
    }

    public void DirectionControl()
    {
        if (moveInput.x > 0.5)
        {
            transform.localScale = new Vector3(1, 1, 1);
            IsRight = true;
        }
        else if (moveInput.x < -0.5)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            IsRight = false;
        }
    }
}
