using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RotateTowardsVelocity : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float rotationOffset = 0f;
    [SerializeField] private bool flipWhenFacingLeft = false;
    [SerializeField] private bool instantRotationOnStart = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool hasInstantRotated = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 velocity = rb.linearVelocity;
        if (velocity.sqrMagnitude < 0.01f)
            return;

        bool facingLeft = velocity.x < 0;
        bool flipped = false;

        if (flipWhenFacingLeft && spriteRenderer != null)
        {
            spriteRenderer.flipY = facingLeft;
            flipped = spriteRenderer.flipY;
        }

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        float effectiveOffset = flipped ? -rotationOffset : rotationOffset;
        float targetAngle = angle + effectiveOffset;

        if (instantRotationOnStart && !hasInstantRotated)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
            hasInstantRotated = true;
        }
        else
        {
            float newZ = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, newZ);
        }
    }
}
