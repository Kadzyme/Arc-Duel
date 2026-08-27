using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] private float damage = 2f;
    [SerializeField] private bool destroyOnHit = true;
    [SerializeField] private bool damageOnce = false;
    [SerializeField] private float tickRate = 0.1f;

    [Header("Particle Settings")]
    [SerializeField] private bool useParticleCollision = false;
    private ParticleSystem targetParticleSystem;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 0f;
    [SerializeField] private Vector2 knockbackOffset = Vector2.zero;

    [Header("Owner Protection")]
    [SerializeField] private float selfDamageIgnoreDuration = 0.2f;

    private readonly List<Health> targets = new();
    private Rigidbody2D rb;
    private List<ParticleCollisionEvent> collisionEvents = new();
    private float currentTickTime = 0f;

    private GameObject owner;
    private float spawnTime;
    private bool isCollider = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
        isCollider = GetComponent<Collider2D>();

        if (useParticleCollision)
            targetParticleSystem = GetComponent<ParticleSystem>();
    }

    public void Init(GameObject shooter)
    {
        owner = shooter;
        spawnTime = Time.time;
    }

    private void Update()
    {
        if (damageOnce || useParticleCollision)
            return;

        currentTickTime -= Time.deltaTime;

        if (targets.Count == 0 || currentTickTime > 0)
            return;

        currentTickTime = tickRate;

        foreach (var target in targets)
        {
            if (target == null) continue;

            if (IsOwnerIgnored(target.gameObject))
                continue;

            target.TakeDamage(damage);
            ApplyKnockback(target, GetProjectileDirection());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (useParticleCollision || isCollider)
            return;

        HandleHit(other.gameObject);
    }

    // Добавили обработку обычных физических столкновений (когда коллайдер НЕ триггер)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (useParticleCollision || !isCollider)
            return;

        HandleHit(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (useParticleCollision || isCollider) return;

        if (other.TryGetComponent(out Health hp))
            targets.Remove(hp);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!useParticleCollision || targetParticleSystem == null)
            return;

        if (!other.TryGetComponent(out Health hp))
            return;

        if (IsOwnerIgnored(other))
            return;

        int numCollisionEvents = targetParticleSystem.GetCollisionEvents(other, collisionEvents);
        if (numCollisionEvents == 0) return;

        for (int i = 0; i < numCollisionEvents; i++)
        {
            hp.TakeDamage(damage);
            Vector2 hitVelocity = collisionEvents[i].velocity;
            ApplyKnockback(hp, hitVelocity);
        }
    }

    // Единый метод для обработки попаданий (и для триггеров, и для коллайдеров)
    private void HandleHit(GameObject hitObject)
    {
        // Если это владелец в первые секунды — вообще не реагируем на столкновение
        if (IsOwnerIgnored(hitObject))
            return;

        bool processed = false;

        // 1. Если у объекта есть здоровье (игроки или другие разрушаемые проджектайлы со скриптом Health)
        if (hitObject.TryGetComponent(out Health hp))
        {
            if (damageOnce)
            {
                hp.TakeDamage(damage);
                ApplyKnockback(hp, GetProjectileDirection());
            }
            else if (!targets.Contains(hp))
            {
                targets.Add(hp);
            }
            processed = true;
        }
        // 2. Если это другой объект с DamageArea
        else if (hitObject.TryGetComponent(out DamageArea otherDamageArea))
        {
            processed = true;
        }
        // 3. Стены и прочие препятствия
        else
        {
            processed = true;
        }

        if (destroyOnHit && processed)
        {
            Global.SafeDestroyProjectile(gameObject);
        }
    }

    private bool IsOwnerIgnored(GameObject targetObj)
    {
        if (owner != null && targetObj == owner && selfDamageIgnoreDuration > 0)
        {
            if (Time.time - spawnTime < selfDamageIgnoreDuration)
            {
                return true;
            }
        }
        return false;
    }

    private void ApplyKnockback(Health target, Vector2 baseDirection)
    {
        if (knockbackForce <= 0f)
            return;

        var targetRb = target.GetComponent<Rigidbody2D>();
        if (targetRb == null)
            return;

        if (baseDirection.sqrMagnitude < 0.0001f)
            baseDirection = transform.right;

        Vector2 dir = (baseDirection.normalized + knockbackOffset).normalized;

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