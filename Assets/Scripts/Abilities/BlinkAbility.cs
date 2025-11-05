using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Dash")]
public class DashAbility : Ability
{
    [SerializeField] private float dashDistance = 5f;

    protected override void Use(GameObject user)
    {
        if (user == null) return;
        Rigidbody2D rb = user.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = user.transform.position.z;
        Vector2 direction = ((Vector2)(mouseWorldPos - user.transform.position)).normalized;
        if (direction == Vector2.zero)
            direction = Vector2.right;

        Collider2D[] colliders = user.GetComponents<Collider2D>();
        foreach (var col in colliders)
            col.enabled = false;

        Vector2 vel = rb.linearVelocity;
        vel.y = 0f;

        user.transform.position = (Vector2)user.transform.position + direction * dashDistance;

        foreach (var col in colliders)
            col.enabled = true;

        rb.linearVelocity = vel;
    }
}
