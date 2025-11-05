using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Blink")]
public class BlinkAbility : Ability
{
    [SerializeField] private float blinkDistance = 5f;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private bool useMouseDirection = true;
    [SerializeField] private bool keepVelocity = false;

    protected override void Use(GameObject user)
    {
        if (user == null) return;

        Vector2 startPos = user.transform.position;
        Vector2 direction;

        if (useMouseDirection)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mousePos - startPos).normalized;
        }
        else
        {
            direction = user.transform.right;
        }

        Vector2 targetPos = startPos + direction * blinkDistance;

        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, blinkDistance, obstacleMask);
        if (hit.collider != null)
            targetPos = hit.point - direction * 0.2f;

        Rigidbody2D rb = user.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 prevVel = rb.linearVelocity;

            rb.MovePosition(targetPos);

            rb.linearVelocity = keepVelocity ? prevVel : Vector2.zero;
        }
        else
        {
            user.transform.position = targetPos;
        }
    }
}
