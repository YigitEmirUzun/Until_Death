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
    WallDetector wallDetector;
    AxolotlHealth healthsc;


    [Header("AttackHitbox")]
    [SerializeField] public Transform UpAttackPosition;
    [SerializeField] public Transform DownAttackPosition;
    [SerializeField] public Transform AttackPosition;
    [SerializeField] public GameObject VerticalAttack;
    [SerializeField] public GameObject HorizontalAttack;

    [Header("Player Situations")]
    [SerializeField] bool IsRight = true;
    [SerializeField] bool Hang = false;
    [SerializeField] public bool moving;
    [SerializeField] public bool Attacking = false;
    [SerializeField] public bool canAttack = true;
    [SerializeField] public bool isDashing = false;
    [SerializeField] public bool canDash = true;
    [SerializeField] public bool Jumping = false;
    [SerializeField] public bool canSecondJump = true;
    public bool interact = false;



    [Header("Player Variables")]
    [SerializeField] public Vector2 moveInput;
    [SerializeField] public float speed;
    [SerializeField] public float JumpForce;


    public enum DirectionState
    {
        Horizontal, Up, Down
    }

    public DirectionState currentDirection = DirectionState.Horizontal;
   
    void Awake()
    {
        action = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        floorDetector = GetComponentInChildren<FloorDetector>();
        wallDetector = GetComponentInChildren<WallDetector>();
        hangDetector = GetComponentInChildren<HangDetector>();
        healthsc = GetComponent<AxolotlHealth>();

        DontDestroyOnLoad(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (floorDetector.OnFloor & !isDashing) { canDash = true; }
        if (floorDetector.OnFloor) { canSecondJump = true; }
    }

    private void FixedUpdate()
    {
        // Hareket
        if((moveInput.x >= 0.6 || moveInput.x <= -0.6) && !isDashing && !Attacking)
        {
            rb.linearVelocityX = moveInput.x * speed;
            moving = true;
        }
        else { moving = false; }

        if(rb.linearVelocityY <= -30)
        {
            rb.linearVelocityY = -30;
        }

        // Yön kontrol fonksiyonu
        DirectionControl();


        // Hang Skilli Fiziksel Hareketi
        if (Hang)
        {
            if(moveInput.y >= 0.6)
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(moveInput.x * 30, 35), ForceMode2D.Impulse);
                Hang = false;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(moveInput.x * 30, moveInput.y * 30), ForceMode2D.Impulse);
                Hang = false;
            }
            
        }

        if(wallDetector.onWall && !floorDetector.OnFloor)
        {
            if (transform.position.x > wallDetector.WallPosition.x)
            {
                IsRight = true;
            }
            else { IsRight = false; }
        }
    }

    private void OnEnable()
    {
        // Inputlarý ekleme
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
        // Inputlarý disable etme
        action.Player.Disable();

        action.Player.Move.performed -= OnMovePerformed;
        action.Player.Move.canceled -= OnMoveCanceled;
        action.Player.Dash.performed -= OnDashPerformed;
        action.Player.Hang.performed -= OnHangPerformed;
        action.Player.Hang.canceled -= OnHangCanceled;
        action.Player.Jump.performed -= OnJumpPerformed;
        action.Player.Attack.performed -= OnAttackPerformed;
    }

    // Move Input
    void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
        if (floorDetector.OnFloor && !isDashing)
        {
            if (IsRight) { rb.linearVelocityX = 5f; }
            else { rb.linearVelocityX = -5f; }
        }
        else if (!isDashing) { rb.linearVelocityX = 0; }
    }

    // Dash Input
    void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (canDash && healthsc.currentStamina > 10)
        {
            isDashing = true;
            healthsc.canRegenStamina = false;
            healthsc.staminaRegenTimer = 0f;
            healthsc.currentStamina -= 20;
            if (IsRight) 
            {
                rb.linearVelocityX = 0;
                rb.AddForce(new Vector2(30, 0), ForceMode2D.Impulse);
                canDash = false;
                StartCoroutine(DashRoutine(0.2f));
            }
            else
            {
                rb.linearVelocityX = 0;
                rb.AddForce(new Vector2(-30, 0), ForceMode2D.Impulse);
                canDash = false;
                StartCoroutine(DashRoutine(0.2f));
            }
        }
    }

    // HangSkill Input
    void OnHangPerformed(InputAction.CallbackContext context)
    {
        if (hangDetector.canHang)
        {
            Time.timeScale = 0.1f;
        }

        interact = true;
    }

    void OnHangCanceled(InputAction.CallbackContext context)
    {
        if (hangDetector.canHang)
        {
            Time.timeScale = 1f;
            rb.linearVelocity = Vector2.zero;
            Hang = true;
        }

        interact = false;
    }

    // Attack Input
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
                currentDirection = DirectionState.Up;
                StartCoroutine(AttackColliderRoutine(HorizontalAttack, UpAttackPosition));
            }
            else if (moveInput.y < -0.4)
            {
                currentDirection = DirectionState.Down;
                StartCoroutine(AttackColliderRoutine(HorizontalAttack, DownAttackPosition));
            }
            else { currentDirection = DirectionState.Horizontal; StartCoroutine(AttackColliderRoutine(VerticalAttack, AttackPosition)); }
        }
        
    }

    // Jump Input
    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (floorDetector.OnFloor)
        {
            rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            Jumping = true;
            StartCoroutine (JumpRoutine());
        }
        else if (canSecondJump)
        {
            canSecondJump = false;
            rb.linearVelocityY = 0;
            rb.AddForce(new Vector2(0, JumpForce + 5f), ForceMode2D.Impulse);
            Jumping = true;
            StartCoroutine(JumpRoutine());
        }
    }

    // Coroutines
    IEnumerator DashRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        isDashing = false;
        rb.linearVelocityX = 0f;
    }

    IEnumerator AttackColliderRoutine(GameObject AttackObject, Transform AttackPosition)
    {
        GameObject NewAttack = Instantiate(AttackObject, AttackPosition.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        GameObject.Destroy(NewAttack);
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        Attacking = false;
        yield return new WaitForSeconds(0.1f);
        canAttack = true;
    }

    IEnumerator JumpRoutine()
    {
        yield return new WaitForSeconds(0.45f);
        Jumping = false;
    }


    // Fonksiyonlar
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
