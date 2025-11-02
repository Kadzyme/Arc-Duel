using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public bool IsGrounded { get; private set; }

    private bool wasGrounded;

    public event System.Action OnLand;
    public event System.Action OnFall;

    private void Update()
    {
        bool groundedNow = Physics2D.OverlapCircle(transform.position, checkRadius, groundLayer);

        if (groundedNow && !wasGrounded)
            OnLand?.Invoke();
        else if (!groundedNow && wasGrounded)
            OnFall?.Invoke();

        IsGrounded = groundedNow;
        wasGrounded = groundedNow;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
