using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Sensor")]
    [SerializeField] private GroundSensor groundSensor;

    private Rigidbody2D rb;
    private bool facingRight = true;
    private float _moveInputX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (groundSensor == null)
            Debug.LogWarning("GroundSensor не назначен в Movement! Прыжки по земле работать не будут.");
    }

    private void Update()
    {
        _moveInputX = 0f;
        if (Input.GetKey(InputManager.Instance.GetKey(PlayerAction.MoveRight))) _moveInputX += 1f;
        if (Input.GetKey(InputManager.Instance.GetKey(PlayerAction.MoveLeft))) _moveInputX -= 1f;

        if (groundSensor != null && groundSensor.IsGrounded &&
            Input.GetKeyDown(InputManager.Instance.GetKey(PlayerAction.Jump)))
        {
            var velocity = rb.linearVelocity;
            velocity.y = jumpForce;
            rb.linearVelocity = velocity;
        }

        if (!facingRight && _moveInputX > 0)
            Flip();
        else if (facingRight && _moveInputX < 0)
            Flip();
    }

    private void FixedUpdate()
    {
        var velocity = rb.linearVelocity;
        velocity.x = _moveInputX * moveSpeed;
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
