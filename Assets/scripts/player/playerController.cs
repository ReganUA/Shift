using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveForce = 50f;      // Сила прискорення
    [SerializeField] private float maxSpeed = 10f;       // Максимальна швидкість
    [SerializeField] private float groundDrag = 3f;      // Тертя на землі (плавність зупинки)
    [SerializeField] private float airDrag = 0.5f;       // Тертя в повітрі

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float gravityMultiplier = 4f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.15f;

    [Header("Detection")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private float raycastOffset = 0.3f;

    public GameObject deathEffect;
    private Rigidbody rb;
    private float horizontalInput;
    private float jumpBufferCounter;
    private float coyoteTimeCounter;
    private bool isGrounded;
    private MeshRenderer playerMesh;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.useGravity = false;
        playerMesh = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        gameManager.instance.player = this;
        gameManager.instance.playerMesh = playerMesh;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Таймери (Coyote & Buffer)
        if (isGrounded) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Jump")) jumpBufferCounter = jumpBufferTime;
        else jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            Jump();
        }

        // Плавне переривання стрибка
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.35f, rb.linearVelocity.z);
            coyoteTimeCounter = 0f;
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            gameManager.instance.ChangeDimenshion();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            sceneManager.ReloadScene();
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
        ApplyMovement();
        ApplyCustomGravity();
        HandleDrag();
    }
    private void ApplyMovement()
    {
        // Додаємо силу замість прямої зміни швидкості
        Vector3 force = Vector3.right * horizontalInput * moveForce;
        rb.AddForce(force, ForceMode.Acceleration);

        // Обмежуємо максимальну швидкість по горизонталі
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (horizontalVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = horizontalVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void HandleDrag()
    {
        // Повертаємо логіку дампінгу (опору) для інерції
        // Якщо гравець не тисне кнопки, він буде ковзати до зупинки
        rb.linearDamping = isGrounded ? groundDrag : airDrag;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;
    }

    private void ApplyCustomGravity()
    {
        rb.AddForce(Vector3.down * 9.81f * gravityMultiplier, ForceMode.Acceleration);
    }

    private void CheckGround()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        isGrounded = Physics.Raycast(origin, Vector3.down, groundCheckDistance + 0.1f, groundLayer) ||
                     Physics.Raycast(origin + Vector3.right * raycastOffset, Vector3.down, groundCheckDistance + 0.1f, groundLayer) ||
                     Physics.Raycast(origin + Vector3.left * raycastOffset, Vector3.down, groundCheckDistance + 0.1f, groundLayer);
    }
    public void SpawnDeathEffect()
    {
        deathEffect.transform.SetParent(null);
        deathEffect.SetActive(true);
    }
}