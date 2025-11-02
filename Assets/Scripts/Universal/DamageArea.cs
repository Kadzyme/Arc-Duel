using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class DamageArea : MonoBehaviour
{
    [SerializeField] private float damagePerTick = 2f;
    [SerializeField] private float tickRate = 0.1f;
    [SerializeField] private bool destroyOnHit = true;
    [SerializeField] private bool damageOnce = false;

    private float tickTimer;
    private readonly List<Health> targets = new();

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
            if (target != null)
                target.TakeDamage(damagePerTick);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Health hp)) 
            return;

        if (!targets.Contains(hp))
            targets.Add(hp);

        if (damageOnce) 
            hp.TakeDamage(damagePerTick);

        if (destroyOnHit) 
            Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Health hp))
            targets.Remove(hp);
    }
}
