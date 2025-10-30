using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class AnimatorController : MonoBehaviour
{
    [Header("Animator Parameters")]
    [SerializeField] private string speedParam = "Speed";
    [SerializeField] private string groundedParam = "IsGrounded";
    [SerializeField] private string jumpingParam = "IsJumping";
    [SerializeField] private string fallingParam = "IsFalling";

    [Header("Ground Sensor")]
    [SerializeField] private GroundSensor groundSensor;

    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (groundSensor == null)
            Debug.LogWarning("GroundSensor не назначен в AnimatorController! Анимации связанные с землёй не будут работать.");
    }

    private void Update()
    {
        float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        float verticalSpeed = rb.linearVelocity.y;

        bool isGrounded = groundSensor != null && groundSensor.IsGrounded;
        bool isJumping = !isGrounded && verticalSpeed > 0.1f;
        bool isFalling = !isGrounded && verticalSpeed < -0.1f;

        if (!string.IsNullOrEmpty(speedParam))
            animator.SetFloat(speedParam, horizontalSpeed);

        if (!string.IsNullOrEmpty(groundedParam))
            animator.SetBool(groundedParam, isGrounded);

        if (!string.IsNullOrEmpty(jumpingParam))
            animator.SetBool(jumpingParam, isJumping);

        if (!string.IsNullOrEmpty(fallingParam))
            animator.SetBool(fallingParam, isFalling);
    }
}
