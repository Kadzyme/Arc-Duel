using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class DamageArea : MonoBehaviour
{
    [SerializeField] private float damagePerTick = 2f;
    [SerializeField] private float tickRate = 0.1f;
    [SerializeField] private bool destroyOnHit = true;
    [SerializeField] private bool damageOnce = false;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 0f;
    [SerializeField] private Vector2 knockbackOffset = Vector2.zero;

    private float tickTimer;
    private readonly List<Health> targets = new();
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (targets.Count == 0 || damageOnce)
            return;

        tickTimer += Time.deltaTime;
        if (tickTimer < tickRate)
            return;

        tickTimer -= tickRate;

        foreach (var target in targets)
        {
            if (target == null) continue;
            target.TakeDamage(damagePerTick);
            ApplyKnockback(target);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Health hp))
            return;

        if (!targets.Contains(hp))
            targets.Add(hp);

        if (damageOnce)
        {
            hp.TakeDamage(damagePerTick);
            ApplyKnockback(hp);
        }

        if (destroyOnHit)
            Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Health hp))
            targets.Remove(hp);
    }

    private void ApplyKnockback(Health target)
    {
        if (knockbackForce <= 0f)
            return;

        var targetRb = target.GetComponent<Rigidbody2D>();
        if (targetRb == null)
            return;

        Vector2 dir = GetProjectileDirection();
        if (dir.sqrMagnitude < 0.0001f)
            dir = transform.right;

        dir = (dir + knockbackOffset).normalized;

        targetRb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
    }

    private Vector2 GetProjectileDirection()
    {
        if (rb == null)
            return transform.right;

        return rb.linearVelocity.sqrMagnitude > 0.01f
            ? rb.linearVelocity.normalized
            : transform.right;
    }
}