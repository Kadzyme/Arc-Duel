using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RotateTowardsVelocity : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 720f;

    private Rigidbody2D rb;

    private void Awake() 
        => rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        if (rb.linearVelocity.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
