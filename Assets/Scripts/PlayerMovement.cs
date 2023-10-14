using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int GetStamina { get { return stamina; } }
    public int SetStamina { set { stamina = value; } }

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float staminaMax;
    [SerializeField] private float stamina;
    [SerializeField] private float staminaCooldown;

    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject gameManager;

    [SerializeField] private int throwAngleRange;
    [SerializeField] private int kickAngleRange;

    private bool canHeal;
    private bool isGrounded;
    private bool isJumping;
    private Rigidbody rb;
    private Animator animator;
    private float horizontalInput;
    private float verticalInput;
    private float timeSinceSprint;
    
    public bool canMove = true;

    private UnitAction unitAction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        unitAction = GetComponent<UnitAction>();
        Debug.Log(Mathf.Sin(((float)unitAction.direction * Mathf.PI) / 180));
        Debug.Log(Mathf.Cos(((float)unitAction.direction * Mathf.PI) / 180));
    }

    private void Update()
    {
        if(canMove)
        {
            // Input handling
            horizontalInput = Mathf.Cos(((float)gameObject.GetComponent<UnitAction>().direction * Mathf.PI) / 180);
            verticalInput = Mathf.Sin(((float)gameObject.GetComponent<UnitAction>().direction * Mathf.PI) / 180);
            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

            // Ground Check
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

            // Rotation
            Rotate(movementDirection);

            // Sprinting
            Sprint();

            // Stamina management
            RefillStamina();

            // Animation
            if(unitAction.type == UnitAction.Types.RUN)
            {
                Animation(movementDirection);
            }
            else
            {
                Animation(new Vector3(0, 0, 0));
            }

            // Jump
            if (isGrounded && unitAction.type == UnitAction.Types.JUMP)
            {
                isJumping = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if(canMove && unitAction.type == UnitAction.Types.RUN)
        {
            // Movement
            Move();
        }
        if(isJumping)
        {
            Jump();
        }
    }

    private void Move()
    {
        if (isGrounded)
        {
            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
            Vector3 moveVelocity = moveDirection * (unitAction.force > 700 && stamina > 0 ? sprintSpeed : movementSpeed);

            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        }
    }

    private void Jump()
    {
        isJumping = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Rotate(Vector3 movementDirection)
    {
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Sprint()
    {
        if (unitAction.type == UnitAction.Types.RUN && unitAction.force > 700 && stamina > 0)
        {
            timeSinceSprint = 0;
            canHeal = false;
            movementSpeed = sprintSpeed;
            stamina -= Time.deltaTime;
        }
        else
        {
            movementSpeed = walkSpeed;
            timeSinceSprint += Time.deltaTime;
        }

        if (timeSinceSprint >= staminaCooldown)
        {
            canHeal = true;
        }

        if (stamina <= 0)
        {
            movementSpeed = walkSpeed;
        }
    }

    private void RefillStamina()
    {
        if (canHeal && stamina < staminaMax)
        {
            stamina += Time.deltaTime * staminaCooldown;
            stamina = Mathf.Clamp(stamina, 0, staminaMax);
        }
    }
    public int ThrowAngleRange()
    {
        throwAngleRange = Mathf.RoundToInt(Random.Range(0, (staminaMax - stamina) * 5));
        return (Random.Range(0, 1) == 0) ? -throwAngleRange : throwAngleRange;
    }
    public int KickAngleRange()
    {
        kickAngleRange = Mathf.RoundToInt(Random.Range(0, (staminaMax - stamina) * 10) + 10);
        return (Random.Range(0, 1) == 0) ? -kickAngleRange : kickAngleRange;
    }

    private void Animation(Vector3 mD)
    {
        if (mD == Vector3.zero)
        {
            animator.SetFloat("speed", 0);
        }
        else if (movementSpeed == sprintSpeed)
        {
            animator.SetFloat("speed", 0.6f);
        }
        else if (movementSpeed == walkSpeed)
        {
            animator.SetFloat("speed", 0.4f);
        }
    }

    public IEnumerator DoMoveFalse(float seconds)
    {
        canMove = false;
        yield return new WaitForSeconds(seconds);
        canMove = true;
    }
    public IEnumerator DoPlayerFall(float seconds)
    {
        canMove = false;
        rb.mass += 100;
        yield return new WaitForSeconds(seconds);
        rb.mass -= 100;
        canMove = true;
    }

    private bool canAdd = true;

    IEnumerator DoAddPoints()
    {
        yield return new WaitForSeconds(2f);
        canAdd = true;
    }
}
