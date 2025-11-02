using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Sensor")]
    [SerializeField] private GroundSensor groundSensor;

    private Rigidbody2D rb;
    private float moveInput;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (groundSensor == null)
            Debug.LogWarning("GroundSensor не назначен в Movement! Прыжки по земле работать не будут.");
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Прыжок
        if (groundSensor != null && groundSensor.IsGrounded && Input.GetButtonDown("Jump"))
        {
            var velocity = rb.linearVelocity;
            velocity.y = jumpForce;
            rb.linearVelocity = velocity;
        }

        // Разворот персонажа
        if (!facingRight && moveInput > 0)
            Flip();
        else if (facingRight && moveInput < 0)
            Flip();
    }

    private void FixedUpdate()
    {
        var velocity = rb.linearVelocity;
        velocity.x = moveInput * moveSpeed;
        rb.linearVelocity = velocity;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
